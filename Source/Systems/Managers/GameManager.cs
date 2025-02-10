using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGui;
using System.Linq;

namespace EndlessSpace
{
    public class GameManager
    {
        readonly Game game;
        bool init = false;

        GraphicsDevice graphics_device;
        GraphicsDeviceManager graphics;
        GameWindow window;

        ImGuiRenderer gui_renderer;

        Background background;

        World world;

        BasicObject cursor;
        UserInterface user_interface;

        public GameManager(Game game, GraphicsDevice graphics_device, GraphicsDeviceManager graphics)
        {
            this.game = game;
            this.graphics_device = graphics_device;
            this.graphics = graphics;
            window = game.Window;

            gui_renderer = new ImGuiRenderer(game).Initialize().RebuildFontAtlas();

            world = new World(graphics_device, window);
            background = new Background(graphics_device, PlayerCharacter);

            user_interface = new UserInterface(game, graphics_device, graphics, world.PlayerCharacter);
            cursor = new BasicObject("Textures\\UI\\pointer_01", Vector2.Zero, new Vector2(22, 31) * 0.5f);
        }

        EntityManager EntityManager => world.EntityManager;
        PlayerCharacter PlayerCharacter => world.PlayerCharacter;
        
        public void Update(GameTime game_time)
        {
            if (!init) init = true;
            else if (MainMenu.active) goto skip;

            world.Update(game_time);
            background.Update(game_time);

            skip:
            user_interface.Update(game_time);

            WorldItems();
        }

        void WorldItems()
        {
            if (is_reset)
            {
                Character.ResetNextID();
                world = new World(graphics_device, window);
                user_interface = new UserInterface(game, graphics_device, graphics, world.PlayerCharacter);
                is_reset = false;
            }

            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.G))
            {
                EntityManager.PassUnit(new NPC(new Frigate(world.PlayerCharacter.Position + new Vector2(100, 100), UnitFaction.Biomantes), EntityManager.Units, 5));
            }
        }


        public void Draw(SpriteBatch sprite_batch, GameTime game_time)
        {
            background.Draw(sprite_batch);
            world.Draw(sprite_batch);
            gui_renderer.BeginLayout(game_time);
            DisplayUnitInfo();
            gui_renderer.EndLayout();
                        
            sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);
            user_interface.Draw(sprite_batch);
            cursor.Draw(sprite_batch, Input.MousePosition, Vector2.Zero, Color.White);
            sprite_batch.End();
        }

        bool is_reset = false;
        private void DisplayUnitInfo()
        {
            if (ImGui.Button("Reset World"))
            {
                is_reset = true;
            }
            ImGui.Text($"{game.IsFixedTimeStep}");
            ImGui.Text("");

            {
                ImGui.Text("PlayerCharacter");
                ImGui.Separator();

                ImGui.Text("Select Faction: ");
                ImGui.SameLine();
                if (ImGui.Button("Biomantes"))
                {
                    PlayerCharacter.SetFaction(UnitFaction.Biomantes);
                }
                ImGui.SameLine();
                if (ImGui.Button("Dusk Fleet"))
                {
                    PlayerCharacter.SetFaction(UnitFaction.DuskFleet);
                }
                ImGui.SameLine();
                if (ImGui.Button("Iron Corpse"))
                {
                    PlayerCharacter.SetFaction(UnitFaction.IronCorpse);
                }
                PlayerXPDebuger(PlayerCharacter);

                ImGui.Text("");
            }


            ImGui.Text("Unit Information");

            foreach (var unit in EntityManager.Units)
            {
                if (unit == null || unit.HasKeyword("asteroid")) return;
                ImGui.Separator();
                var character = unit as Character;
                ImGui.Text($"Name: {unit.Name}, ID: {character?.ID}, Level: {unit.Level}");
                ImGui.Text("");
                ImGui.Text($"Faction: {unit.Faction}, IsBoss: {(unit as NPC)?.IsBoss}");
                ImGui.Text($"IsPlayerTeammate: {character?.IsPlayerTeammate}");
                ImGui.Text($"IsDead: {unit.IsDead} IsDestroyed: {unit.IsDestroyed}");
                ImGui.Text($"X: {unit.Position.X}, Y: {unit.Position.Y}");

                ImGui.Text("");
                ImGui.Text($"Health: {unit.GetUnitValue(UnitValue.Health)}");
                ImGui.Text($"Heal: {unit.GetUnitValue(UnitValue.Heal)}");
                ImGui.Text($"HealRate: {unit.GetUnitValue(UnitValue.HealRate)}");
                ImGui.Text($"CriticalChance: {unit.GetUnitValue(UnitValue.CriticalChance)}");
                ImGui.Text($"Magnitude: {unit.GetUnitValue(UnitValue.Magnitude)}");
                ImGui.Text($"DamageResist: {unit.GetUnitValue(UnitValue.DamageResist)}");
                ImGui.Text($"SpeedMult: {unit.GetUnitValue(UnitValue.SpeedMult)}");
                ImGui.Text("");

                if (unit.Keywords.Count > 0)
                {
                    ImGui.Text("Keywords:");
                    foreach (var keyword in unit.Keywords)
                    {
                        ImGui.Text($"{keyword}");
                    }
                    ImGui.Text("");
                }

                if (unit.Skills.Count > 0)
                {
                    ImGui.Text("Skills:");
                    foreach (var skill in unit.Skills)
                    {
                        ImGui.Text($"{skill.Name}, Tag: {skill.Tags.FirstOrDefault()}");
                    }
                    ImGui.Text("");
                }

                if (unit.EffectTarget.ActiveEffects.Count > 0)
                {
                    ImGui.Text("Active Effects:");
                    foreach (var effect in unit.EffectTarget.ActiveEffects)
                    {
                        ImGui.Text($"{effect.Name}");
                    }
                    ImGui.Text("");
                }

                if (unit is NPC npc)
                {
                    var target = npc.current_target as Character;
                    if (target != null)
                    {
                        ImGui.Text($"current target: {target.Name}, ID: {target.ID}");
                        ImGui.Text("");
                    }
                    if (npc.detected_units.Count > 0)
                    {
                        ImGui.Text("detected units:");
                        foreach (var detected_unit in npc.detected_units)
                        {
                            var detected_char = detected_unit as Character;
                            ImGui.Text($"{detected_char?.Name}, ID: {detected_char?.ID}");
                        }
                        ImGui.Text("");
                    }
                }

                ImGui.Text("");
            }
        }

        private void PlayerXPDebuger(PlayerCharacter player)
        {
            ImGui.Text($"Level {player.Level}, Exp {player.Experience.CurrentXP}/{player.Experience.NextXP}");
            ImGui.SameLine();
            if (ImGui.Button("+"))
            {
                player.Experience.AddExp(1);
            }
            ImGui.SameLine();
            if (ImGui.Button("-"))
            {
                player.Experience.AddExp(-1);
            }
            ImGui.SameLine();
            if (ImGui.Button("NextLevel"))
            {
                player.Experience.AddExp(player.Experience.NextXP);
            }
            ImGui.SameLine();
            if (ImGui.Button("PrevLevel"))
            {
                --player.Level;
            }

            if (ImGui.Button("UnKillable"))
            {
                PlayerCharacter.UnKillable();
            }
            ImGui.SameLine();
            ImGui.Text($"{PlayerCharacter.IsUnKillable}");
            if (ImGui.Button("Rotation To Zero"))
            {
                PlayerCharacter.Rotation = 0f;
            }
        }
    }
}
