using EndlessSpace;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using System.Collections.Generic;
using MonoGame.Extended;

public class Slider : ISettingsElement
{
    string title;
    float value;
    SizeF title_size, value_size;
    BitmapFont font;

    Button decrease_button, increase_button;
    List<Button> button_list = new List<Button>();

    Vector2 title_position, value_position;
    float offset;

    public Slider(string title, ref float value, RectangleF rect, float offset)
    {
        this.title = title;
        this.value = value;
        this.offset = offset;
        font = Font.CourierNew24;

        title_size = font.MeasureString(title);
        value_size = font.MeasureString(value.ToString("0.0"));

        var button_size = new SizeF(16, 26);
        decrease_button = new Button("Textures\\UI\\arrow_01", Vector2.Zero, button_size, new Color(100, 100, 100), null, null);
        decrease_button.have_timer = true;
        increase_button = new Button("Textures\\UI\\arrow_00", Vector2.Zero, button_size, new Color(100, 100, 100), null, null);
        increase_button.have_timer = true;

        button_list.Add(decrease_button);
        button_list.Add(increase_button);

        decrease_button.on_active += () =>
        {
            this.value -= 0.1f;
            RangedValue();
        };
        increase_button.on_active += () =>
        {
            this.value += 0.1f;
            RangedValue();
        };

        UpdatePosition(rect);
    }

    public Vector2 Size
    {
        get
        {
            return new Vector2(value_size.Width, value_size.Height);
        }
    }

    public void UpdatePosition(RectangleF rect)
    {
        float y = rect.Y + offset;
        float x = rect.Width / 2;

        float title_x = rect.X + (x / 2) - (title_size.Width / 2);
        title_position = new Vector2(title_x, y);

        float value_x = rect.X + x + (x / 2) - (value_size.Width);
        float button_y = y + value_size.Height / 2;

        decrease_button.Position = new Vector2(value_x - decrease_button.Size.X, button_y);
        increase_button.Position = new Vector2(value_x + value_size.Width + increase_button.Size.X, button_y);
        value_position = new Vector2(value_x, y);
    }

    void RangedValue()
    {
        if (value < 0f) value = 0f;
        else if (value > 1f) value = 1f;
    }

    public void Update(GameTime game_time)
    {
        button_list.ForEach(b => b.Update(game_time));
    }

    public void Draw(SpriteBatch sprite_batch)
    {
        button_list.ForEach(b => b.Draw(sprite_batch));

        sprite_batch.DrawString(font, title, title_position, Color.White);

        sprite_batch.DrawString(font, value.ToString("0.0"), value_position, Color.White);
    }
}
