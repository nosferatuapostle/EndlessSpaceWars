using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using System;

namespace EndlessSpace
{
    public class CheckBox : ISettingsElement
    {
        Func<bool> get_value;
        Action<bool> set_value;

        string title;
        BitmapFont font;

        Button button;
        RectangleF box;

        Vector2 title_position;
        float offset, offset_y;

        public CheckBox(string title, Func<bool> get_value, Action<bool> set_value, RectangleF rect, float offset_y)
        {
            this.title = title;
            this.offset_y = offset_y;
            this.get_value = get_value;
            this.set_value = set_value;
            font = Font.CourierNew24;

            var title_size = font.MeasureString(title);
            button = new Button(null, Vector2.Zero, new Vector2(title_size.Height, title_size.Height), new Color(125, 125, 125), null, string.Empty);
            button.outline_rect = true;

            offset = button.Size.Y * 0.25f;
            box = new RectangleF(0, 0, button.Size.X - offset, button.Size.Y - offset);

            UpdatePosition(rect);

            button.on_active += () =>
            {
                set_value(!get_value());
            };
        }

        public Vector2 Size
        {
            get
            {
                return button.Size;
            }
        }

        public void UpdatePosition(RectangleF rect)
        {
            float x = rect.X + rect.Width / 2f;
            float y = rect.Y + offset_y;

            float title_x = x - (rect.Width / 4f) - (font.MeasureString(title).Width / 2f);
            float title_y = y - (font.MeasureString(title).Height / 2f);

            title_position = new Vector2(title_x, title_y);

            float button_x = x + (rect.Width / 4f);

            button.Position = new Vector2(button_x, y);
            box.Position = new Vector2(button_x - button.Size.X / 2f + offset / 2f, y - button.Size.Y / 2f + offset / 2f);
        }

        public void Update(GameTime game_time)
        {
            button.Update(game_time);
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            button.Draw(sprite_batch);
            sprite_batch.DrawString(font, title, title_position, Color.White);
            if (get_value()) sprite_batch.FillRectangle(box, new Color(160, 205, 50) * 0.75f);
        }
    }
}
