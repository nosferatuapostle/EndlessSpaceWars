using EndlessSpace;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Timers;
using Microsoft.Xna.Framework.Input;

public class MainMenu
{
    readonly MouseListener mouse_listener;
    readonly KeyboardListener keyboard_listener;

    const string TEXT = "Press any button to start!";
    public static bool active;
    BitmapFont font;

    GraphicsDevice graphics_device;

    Button settings_button;
    
    bool show_text;
    CountdownTimer timer;

    public MainMenu(GraphicsDevice graphics_device)
    {
        active = true;
        show_text = true;
        font = Font.CourierNew24Bold;

        timer = new CountdownTimer(0.5f);
        mouse_listener = new MouseListener();
        keyboard_listener = new KeyboardListener();

        this.graphics_device = graphics_device;

        settings_button = new Button("Textures\\UI\\icon_00", Vector2.Zero, new Vector2(510, 510), new Color(215, 215, 215) * 0.5f, null, string.Empty);
        settings_button.Size *= 0.06f;
                
        settings_button.on_active += () => SettingsMenu.active = true;

        mouse_listener.MouseUp += (s, e) =>
        {
            if (!settings_button.Clicked(e) && !SettingsMenu.active) active = false;
        };
        keyboard_listener.KeyReleased += (s, e) =>
        {
            if (SettingsMenu.active || StatsMenu.active) return;
            if (e.Key == Keys.Escape) SettingsMenu.active = true;
            else active = false;
        };
        
        UpdateLayout(graphics_device);
    }

    public void UpdateLayout(GraphicsDevice graphics_device)
    {
        float offset = graphics_device.Viewport.Height * 0.01f;
        settings_button.Position = new Vector2(graphics_device.Viewport.Width - settings_button.Size.X / 2f - offset, settings_button.Size.Y / 2f + offset);
    }

    public void Update(GameTime game_time)
    {
        mouse_listener.Update(game_time);
        keyboard_listener.Update(game_time);

        if (SettingsMenu.active || StatsMenu.active) return;
        settings_button.Update(game_time);

        if (!active) return;
        timer.Update(game_time);
        if (timer.State == TimerState.Completed)
        {
            show_text = !show_text;
            timer.Restart();
        }
    }

    public void Draw(SpriteBatch sprite_batch)
    {
        SizeF text_size = font.MeasureString(TEXT);
        if (SettingsMenu.active || StatsMenu.active) return;
        settings_button.Draw(sprite_batch);

        if (!active) return;
        if (show_text)
        {
            sprite_batch.DrawString(font, TEXT, new Vector2(graphics_device.Viewport.Width / 2f - text_size.Width / 2f, graphics_device.Viewport.Height / 1.2f - text_size.Height / 2f), Color.AliceBlue * 0.8f);
        }
    }
}
