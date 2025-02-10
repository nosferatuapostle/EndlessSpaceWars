using EndlessSpace;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using System;
using MonoGame.Extended;

public class Selector : ISettingsElement
{
    public event Action on_changed;

    private Func<int> get_value;
    private Action<int> set_value;
    private string[] options;
    private string title;
    private int selected_index;
    float offset_y;

    Button left_button, right_button;
    Vector2 title_position, value_position;
    BitmapFont font;

    public Selector(string title, string[] options, Func<int> get_value, Action<int> set_value, RectangleF rect, float offset_y)
    {
        this.title = title;
        this.options = options;
        this.offset_y = offset_y;
        this.get_value = get_value;
        this.set_value = set_value;
        this.selected_index = get_value();
        font = Font.CourierNew24;

        var title_size = font.MeasureString(title);
        var button_size = new Vector2(title_size.Height, title_size.Height);

        left_button = new Button("Textures/UI/arrow_01", Vector2.Zero, button_size, Color.Gray, null, null);
        right_button = new Button("Textures/UI/arrow_00", Vector2.Zero, button_size, Color.Gray, null, null);

        left_button.on_active += () =>
        {
            selected_index = (selected_index - 1 + options.Length) % options.Length;
            set_value(selected_index);
            on_changed?.Invoke();
        };

        right_button.on_active += () =>
        {
            selected_index = (selected_index + 1) % options.Length;
            set_value(selected_index);
            on_changed?.Invoke();
        };

        UpdatePosition(rect);
    }

    public Vector2 Size
    {
        get
        {
            return new Vector2(left_button.Size.X, left_button.Size.Y);
        }
    }

    public void UpdatePosition(RectangleF rect)
    {
        float x = rect.X + rect.Width / 2f;
        float y = rect.Y + offset_y;

        var title_size = font.MeasureString(title);
        var value_size = font.MeasureString(options[selected_index]);
        var button_size = left_button.Size;

        title_position = new Vector2(x - rect.Width / 4f - title_size.Width / 2f, y);

        value_position = new Vector2(x + rect.Width / 4f - value_size.Width / 2f, y);

        left_button.Position = new Vector2(value_position.X - button_size.X, y + button_size.Y / 2f);
        right_button.Position = new Vector2(value_position.X + value_size.Width + button_size.X, y + button_size.Y / 2f);
    }



    public void Update(GameTime game_time)
    {
        left_button.Update(game_time);
        right_button.Update(game_time);
    }

    public void Draw(SpriteBatch sprite_batch)
    {
        sprite_batch.DrawString(font, title, title_position, Color.White);
        sprite_batch.DrawString(font, options[selected_index], value_position, Color.White);

        left_button.Draw(sprite_batch);
        right_button.Draw(sprite_batch);
    }
}
