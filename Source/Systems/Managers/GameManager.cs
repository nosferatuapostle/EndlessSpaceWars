using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGui;

namespace EndlessSpace
{
    public class GameManager
    {
        GraphicsDevice graphics_device;
        GameWindow window;

        ImGuiRenderer gui_renderer;

        Background background;

        World world;

        BasicObject cursor;
        UserInterface user_interface;

        public GameManager(Game game, GraphicsDevice graphics_device, GameWindow window)
        {
            this.graphics_device = graphics_device;
            this.window = window;

            gui_renderer = new ImGuiRenderer(game).Initialize().RebuildFontAtlas();

            background = new Background(graphics_device);

            world = new World(graphics_device, window);

            cursor = new BasicObject("Textures\\UI\\PointerFilled", Vector2.Zero, new Vector2(22, 31) * 0.5f);
            user_interface = new UserInterface(world.PlayerCharacter);
        }

        EntityManager EntityManager => world.EntityManager;
        PlayerCharacter PlayerCharacter => world.PlayerCharacter;

        public void Update(GameTime game_time)
        {
            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.R) || is_reset)
            {
                Character.ResetNextID();
                world = new World(graphics_device, window);
                user_interface = new UserInterface(world.PlayerCharacter);
                is_reset = false;
            }

            /*if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.D5))
                EntityManager.AddUnit(new NPC(new Scout(new Vector2(-200, -200), UnitFaction.Biomantes), EntityManager.UnitList));*/

            world.Update(game_time);

            user_interface.Update(graphics_device, game_time);
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
            /*float new_x = world.pos.X;
            float new_y = world.pos.Y;
            ImGui.Text($"Position:\n");
            if (ImGui.InputFloat($"X", ref new_x, 2f))
            {
                world.pos = new Vector2(new_x, new_y);
            }
            if (ImGui.InputFloat($"Y", ref new_y, 2f))
            {
                world.pos = new Vector2(new_x, new_y);
            }*/

            if (ImGui.Button("Reset World"))
            {
                is_reset = true;
            }
            ImGui.Text($"Camera: X - {World.Camera.BoundingRectangle.X}, Y - {World.Camera.BoundingRectangle.Y}");
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

            foreach (var unit in EntityManager.UnitList)
            {
                ImGui.Separator();
                var character = unit as Character;
                ImGui.Text($"Name: {unit.Name}, ID: {character.ID}, Level: {character.Level}");
                ImGui.Text("");
                ImGui.Text($"Faction: {unit.Faction}");
                ImGui.Text($"IsPlayerTeammate: {character.IsPlayerTeammate}");
                ImGui.Text($"IsDead: {character.IsDead}");
                ImGui.Text($"X: {character.Position.X}, Y: {character.Position.Y}");

                ImGui.Text("");
                ImGui.Text($"Health: {character.GetUnitValue(UnitValue.Health)}");
                ImGui.Text($"Heal: {character.GetUnitValue(UnitValue.Heal)}");
                ImGui.Text($"HealRate: {character.GetUnitValue(UnitValue.HealRate)}");
                ImGui.Text($"CriticalChance: {character.GetUnitValue(UnitValue.CriticalChance)}");
                ImGui.Text($"Magnitude: {character.GetUnitValue(UnitValue.Magnitude)}");
                ImGui.Text($"DamageResist: {character.GetUnitValue(UnitValue.DamageResist)}");
                ImGui.Text($"SpeedMult: {character.GetUnitValue(UnitValue.SpeedMult)}");
                ImGui.Text("");

                if (unit.EffectTarget.ActiveEffects.Count > 0)
                {
                    ImGui.Text("Active Effects:");
                    foreach (UnitEffect effect in unit.EffectTarget.ActiveEffects)
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
                            ImGui.Text($"{detected_char.Name}, ID: {detected_char.ID}");
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
