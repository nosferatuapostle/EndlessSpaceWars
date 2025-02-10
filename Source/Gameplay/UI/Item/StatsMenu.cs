using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Timers;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class StatsMenu
    {
        KeyboardListener keyboard_listener;

        PlayerCharacter player;

        public static bool active;
        bool is_open;
        BitmapFont font;

        RectangleF rect, info_rect;
        Button stats_button, player_button, skills_button, effects_button, close_button, back_button;

        List<Button> main_list = new List<Button>();

        Skill selected_skill;
        string skill_description = "";
        List<Button> player_skills = new List<Button>();
        
        UnitEffect selected_effect;
        string effect_description = "";
        List<Button> player_effects = new List<Button>();

        List<string> player_values = new List<string>();

        public StatsMenu(PlayerCharacter player, GraphicsDevice graphics_device)
        {
            keyboard_listener = new KeyboardListener();

            this.player = player;

            is_open = false;
            active = false;
            font = Font.CourierNew24;

            stats_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), Font.CourierNew24, "Stats");
            stats_button.Size = font.MeasureString(stats_button.text);

            close_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), Font.CourierNew24, "X");
            close_button.Size = font.MeasureString(close_button.text);

            back_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), Font.CourierNew24, "<<");
            back_button.Size = font.MeasureString(back_button.text);

            player_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), Font.CourierNew24, "Stats");
            player_button.Size = font.MeasureString(player_button.text);

            skills_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), Font.CourierNew24, "Skills");
            skills_button.Size = font.MeasureString(skills_button.text);

            effects_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), Font.CourierNew24, "Effects");
            effects_button.Size = font.MeasureString(effects_button.text);

            main_list.Add(close_button);
            main_list.Add(player_button);
            main_list.Add(skills_button);
            main_list.Add(effects_button);

            UpdateLayout(graphics_device);

            stats_button.on_active += () => active = true;
            close_button.on_active += () => active = false;
            back_button.on_active += () => is_open = false;

            player_button.on_active += () =>
            {
                main_list.ForEach(b => b.is_selected = false);
                is_open = true;
                player_button.is_selected = true;
                player_values.Clear();
                player_values.Add($"Health: {player.GetBaseUnitValue(UnitValue.Health)}");
                player_values.Add($"Heal: {player.GetBaseUnitValue(UnitValue.Heal)}");
                player_values.Add($"HealRate: {player.GetBaseUnitValue(UnitValue.HealRate)}");
                player_values.Add($"CriticalChance: {player.GetBaseUnitValue(UnitValue.CriticalChance)}");
                player_values.Add($"Magnitude: {player.GetBaseUnitValue(UnitValue.Magnitude)}");
                player_values.Add($"DamageResist: {player.GetBaseUnitValue(UnitValue.DamageResist)}");
                player_values.Add($"SpeedMult: {player.GetBaseUnitValue(UnitValue.SpeedMult)}");
            };

            skills_button.on_active += () =>
            {
                main_list.ForEach(b => b.is_selected = false);
                is_open = true;
                skills_button.is_selected = true;
                player_skills.Clear();
                foreach (var skill in player.Skills)
                {
                    var button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), Font.CourierNew18, skill.Name);
                    button.Size = font.MeasureString(button.text);

                    button.on_active += () =>
                    {
                        selected_skill = skill;
                        skill_description = skill.Name;
                    };

                    player_skills.Add(button);
                }

                float offset_y = info_rect.Height * 0.05f;

                foreach (var button in player_skills)
                {
                    button.Position = new Vector2(info_rect.X + info_rect.Width / 2f, info_rect.Top + offset_y * 2f);
                    offset_y += button.Size.Y;
                }
            };
            
            effects_button.on_active += () =>
            {
                main_list.ForEach(b => b.is_selected = false);
                is_open = true;
                effects_button.is_selected = true;
                player_effects.Clear();
                foreach (var effect in player.EffectTarget.ActiveEffects)
                {
                    var button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), Font.CourierNew18, effect.Name);
                    button.Size = font.MeasureString(button.text);

                    button.on_active += () =>
                    {
                        selected_effect = effect;
                        effect_description = effect.Name;
                    };

                    player_effects.Add(button);
                }

                float offset_y = info_rect.Height * 0.05f;

                foreach (var button in player_effects)
                {
                    button.Position = new Vector2(info_rect.X + info_rect.Width / 2f, info_rect.Top + offset_y * 2f);
                    offset_y += button.Size.Y;
                }
            };

            keyboard_listener.KeyReleased += (s, e) =>
            {
                if (e.Key == Keys.Escape)
                {
                    if (is_open) is_open = false;
                    else active = false;
                }
            };
        }

        public void UpdateLayout(GraphicsDevice graphics_device)
        {
            rect = graphics_device.Viewport.Bounds.ToRectangleF();
            float width = rect.Width * 0.32f;
            float height = rect.Height * 0.32f;
            float x = rect.X + rect.Width / 2f - width / 2f;
            float y = rect.Y + rect.Height / 2f - height / 2f;
            rect = new RectangleF(x, y, width, height);

            width = rect.Width * 2f;
            height *= 2f;
            x = rect.X + rect.Width / 2f - width / 2f;
            y = rect.Y + rect.Height / 2f - height / 2f;
            info_rect = new RectangleF(x, y, width, height);

            stats_button.Position = new Vector2(graphics_device.Viewport.Width - stats_button.Size.X, graphics_device.Viewport.Height - stats_button.Size.Y);
            close_button.Position = new Vector2(rect.Right - close_button.Size.X / 2f, rect.Top + close_button.Size.Y / 2f);
            back_button.Position = new Vector2(info_rect.Left + back_button.Size.X / 2f, info_rect.Top + back_button.Size.Y / 2f);

            float offset_y = rect.Height * 0.05f;
            float button_x = rect.X + rect.Width / 2f;

            float button_size = font.MeasureString(player_button.text).Height / 2f;

            player_button.Position = new Vector2(button_x, rect.Top + button_size + offset_y * 2f);
            offset_y += player_button.Size.Y;
            skills_button.Position = new Vector2(button_x, rect.Top + button_size + offset_y * 2f);
            offset_y += skills_button.Size.Y;
            effects_button.Position = new Vector2(button_x, rect.Top + button_size + offset_y * 2f);

            offset_y = info_rect.Height * 0.05f;
            foreach (var button in player_effects)
            {
                button.Position = new Vector2(info_rect.X + info_rect.Width / 2f, info_rect.Top + offset_y * 2f);
                offset_y += button.Size.Y;
            }

            offset_y = info_rect.Height * 0.05f;
            foreach (var button in player_effects)
            {
                button.Position = new Vector2(info_rect.X + info_rect.Width / 2f, info_rect.Top + offset_y * 2f);
                offset_y += button.Size.Y;
            }
        }

        public void Update(GameTime game_time)
        {
            keyboard_listener.Update(game_time);
            if (is_open)
            {
                if (skills_button.is_selected)
                {
                    player_skills.ForEach(b => b.Update(game_time));
                }
                else if (effects_button.is_selected)
                {
                    player_effects.ForEach(b => b.Update(game_time));
                }
                back_button.Update(game_time);
                return;
            }
            if (active)
            {
                main_list.ForEach(b => b.Update(game_time));
                return;
            }
            stats_button.Update(game_time);
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            if (is_open)
            {
                sprite_batch.FillRectangle(info_rect, Color.Black * 0.75f);
                if (player_button.is_selected)
                {
                    float offset_y = info_rect.Top + 80;
                    foreach (var value in player_values)
                    {
                        sprite_batch.DrawString(font, value, new Vector2(info_rect.X + 20, offset_y), Color.White);
                        offset_y += font.LineHeight;
                    }
                }
                else if (skills_button.is_selected)
                {
                    player_skills.ForEach(b => b.Draw(sprite_batch));

                    if (!string.IsNullOrEmpty(skill_description))
                    {
                        var text_size = Font.CourierNew18.MeasureString(skill_description);
                        var desc_position = new Vector2(info_rect.X, info_rect.Bottom - text_size.Height);
                        sprite_batch.DrawString(Font.CourierNew18, skill_description, desc_position, Color.White);
                    }
                }
                else if (effects_button.is_selected)
                {
                    player_effects.ForEach(b => b.Draw(sprite_batch));

                    if (!string.IsNullOrEmpty(effect_description))
                    {
                        var text_size = Font.CourierNew18.MeasureString(effect_description);
                        var desc_position = new Vector2(info_rect.X, info_rect.Bottom - text_size.Height);
                        sprite_batch.DrawString(Font.CourierNew18, effect_description, desc_position, Color.White);
                    }
                }
                back_button.Draw(sprite_batch);
                return;
            }
            if (active)
            {
                sprite_batch.FillRectangle(rect, Color.Black * 0.75f);
                main_list.ForEach(b => b.Draw(sprite_batch));
                return;
            }
            stats_button.Draw(sprite_batch);
        }
    }
}
