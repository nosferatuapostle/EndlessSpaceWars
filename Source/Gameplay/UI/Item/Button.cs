using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Timers;
using System;

namespace EndlessSpace
{
    public class Button : BasicObject
    {
        public event Action on_active;

        public bool no_rectangle, outline_rect, is_selected, have_timer;
        public BitmapFont font;
        public string text;

        bool is_pressed, is_hovered;
        Color color;

        CountdownTimer tick_time;

        public Button(string path, Vector2 position, Vector2 size, Color color, BitmapFont font, string text) : base(path, position, size)
        {
            this.text = text;
            this.color = color;
            
            no_rectangle = true;
            is_selected = false;
            have_timer = false;

            is_hovered = false;
            is_pressed = false;

            tick_time = new CountdownTimer(0.2f);

            if (font == null || text == null) return;
            this.font = font;
        }

        public bool Clicked(MouseEventArgs e)
        {
            return IsHovered() && e.Button == MouseButton.Left && is_pressed;
        }

        public void Update(GameTime game_time)
        {
            if (IsHovered())
            {
                is_hovered = true;
                if (Input.WasLeftClick)
                {
                    is_hovered = false;
                    is_pressed = true;
                }
                else if (Input.LeftClickDone && is_pressed && !have_timer) on_active?.Invoke();
            }
            else is_hovered = false;

            if (!Input.IsLeftClick && !Input.WasLeftClick) is_pressed = false;

            if (!have_timer) return;

            tick_time.Update(game_time);
            if (is_pressed && tick_time.State == TimerState.Completed)
            {
                on_active?.Invoke();
                tick_time.Restart();
            }
        }

        public override RectangleF GetRectangle()
        {
            return new RectangleF(Position - Size / 2f, Size);
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            Color temp_color = color;
            if (is_pressed) temp_color = new Color(color.R - 40, color.G - 40, color.B - 40);
            else if (is_hovered) temp_color = new Color(color.R + 40, color.G + 40, color.B + 40);
            Color = temp_color;

            base.Draw(sprite_batch);

            if (!no_rectangle) sprite_batch.FillRectangle(GetRectangle(), temp_color);
            if (outline_rect) sprite_batch.DrawRectangle(GetRectangle(), temp_color);
            if (font == null || text == null) return;

            var current_font = font;
            if (is_selected)
            {
                if (font == Font.CourierNew16) current_font = Font.CourierNew16Bold;
                else if (font == Font.CourierNew18) current_font = Font.CourierNew18Bold;
                else if (font == Font.CourierNew24) current_font = Font.CourierNew24Bold;
            }

            Color text_color = color.Invert();

            if (no_rectangle || outline_rect)
            {
                if (is_pressed) text_color = new Color(text_color.R - 40, text_color.G - 40, text_color.B - 40);
                else if (is_hovered) text_color = new Color(text_color.R + 40, text_color.G + 40, text_color.B + 40);
                else if (is_selected) text_color = new Color(text_color.R + 20, text_color.G + 20, text_color.B + 20);
            }

            Vector2 text_size = font.MeasureString(text);
            sprite_batch.DrawString(current_font, text, Position + new Vector2(-text_size.X / 2f, -text_size.Y / 2f), text_color);
        }
    }
}
