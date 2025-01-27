using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EndlessSpace
{
    public enum UnitFaction
    {
        None,

        Biomantes,
        DuskFleet,
        IronCorpse,

        Summoned
    }

    public class UnitInfo
    {
        Unit unit;
        List<Unit> unit_list;
        PlayerCharacter player;
                
        Vector2 unit_size, position, target_position;
        string text;
        BitmapFont font;
        Color color;

        HealthBar health_bar;

        List<FloatingDamage> floating_damage = new List<FloatingDamage>();

        public UnitInfo(Unit unit, List<Unit> unit_list)
        {
            this.unit = unit;
            this.unit_list = unit_list;

            text = string.Empty;
            color = Color.White;

            font = Globals.Content.Load<BitmapFont>("Fonts/CourierNew16");

            health_bar = new HealthBar(unit);

            unit_size = unit.Size * unit.Scale;
            position = new Vector2((float)Math.Round(unit.Position.X - unit_size.X / 2f), (float)Math.Round(unit.Position.Y - unit_size.X / 2f - 10f));
        }

        public void AddFloatingDamage(float damage, Color color)
        {
            string damage_text = Math.Round(damage).ToString();
            SizeF text_size = font.MeasureString(damage_text);
            Vector2 text_position = new Vector2(
                unit.Position.X - text_size.Width / 2f,
                unit.Position.Y - unit_size.Y / 2f - 24f
            );
            floating_damage.Add(new FloatingDamage(damage_text, text_position, color));
        }

        public virtual void Update(GameTime game_time)
        {
            if (player == null) player = unit_list.OfType<PlayerCharacter>().FirstOrDefault();

            health_bar.Update(game_time.GetElapsedSeconds());
            floating_damage.RemoveAll(f => f.Update(game_time));

            if (player != null && unit != player && unit is Character character)
            {
                if (!character.IsPlayerTeammate && character.HostileTo(player))
                {
                    int level_difference = unit.Level - player.Level;

                    if (level_difference >= 18)
                        color = Color.Purple;
                    else if (level_difference >= 10)
                        color = Color.Red;
                    else if (level_difference >= 2)
                        color = Color.Orange;
                    else if (level_difference >= -6)
                        color = Color.Gray;
                    else
                        color = Color.LightSteelBlue;
                }
                else if (character != player && !character.IsPlayerTeammate && !character.HostileTo(player))
                {
                    color = Color.CornflowerBlue;
                }
                else
                {
                    color = Color.DarkSeaGreen;
                }
            }
            else
            {
                color = Color.White;
            }
            
            text = $"{unit.Name} {unit.Level} lvl";
            SizeF text_size = font.MeasureString(text);
            float y = unit.Position.Y - 10f;
            if (unit.Faction == UnitFaction.Summoned)
                y = unit.Position.Y - 32f;
            target_position = new Vector2((float)Math.Round(unit.Position.X - text_size.Width / 2f), (float)Math.Round(y - unit_size.Y / 2f));

            position = target_position;
        }

        public virtual void Draw(SpriteBatch sprite_batch)
        {
            /*CircleF circle = (CircleF)unit.Bounds;
            sprite_batch.DrawRectangle(unit.GetRectangle(), Color.White);
            sprite_batch.DrawCircle(circle, 64, Color.Red);

            if (unit is NPC npc)
            {
                string text = string.Join(", ", npc.detected_units.Select(u => u.Name));
                SizeF text_size = font.MeasureString(text);

                float y = unit.Position.Y;
                if (unit.Faction == UnitFaction.Summoned)
                    y = unit.Position.Y + 20f;

                Vector2 text_position = new Vector2((float)Math.Round(unit.Position.X - text_size.Width / 2f), (float)Math.Round(y + unit.Size.Y / 2f));

                if (!string.IsNullOrEmpty(text))
                {
                    sprite_batch.DrawString(font, text, text_position, Color.White);
                }

                sprite_batch.DrawCircle(npc.detection_radius, 100, Color.Green * 0.3f, 1f);
            }*/

            if (!unit.IsDead)
            {
                sprite_batch.DrawString(font, text, position, color);

                if (unit.GetUnitValue(UnitValue.Health) < unit.GetBaseUnitValue(UnitValue.Health) && unit.Faction != UnitFaction.Summoned)
                    health_bar.Draw(sprite_batch);
            }

            foreach (FloatingDamage floating_damage in floating_damage)
                floating_damage.Draw(sprite_batch, font);
        }
    }
}
