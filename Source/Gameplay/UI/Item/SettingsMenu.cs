using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    interface ISettingsElement
    {
        void UpdatePosition(RectangleF rect);
        void Update(GameTime game_time);
        void Draw(SpriteBatch sprite_batch);
    }

    public class SettingsMenu
    {
        readonly MouseListener mouse_listener;
        readonly KeyboardListener keyboard_listener;

        public static bool active;

        RectangleF rect, white_rect;

        BitmapFont font;
        Button settings_button;

        Button game_button, sound_button, graphics_button, close_button;
        List<Button> tab_list;

        Button pause_button, exit_button;
        List<Button> game_list;

        Slider volume_slider, music_slider, sound_slider;
        List<Slider> sound_list;
        
        CheckBox vsync_box;
        Button apply_button;
        List<ISettingsElement> graphics_list;

        Selector selector;

        public SettingsMenu(Game game, GraphicsDevice graphics_device, GraphicsDeviceManager graphics)
        {
            mouse_listener = new MouseListener();
            keyboard_listener = new KeyboardListener();
            tab_list = new List<Button>();

            game_list = new List<Button>();
            sound_list = new List<Slider>();
            graphics_list = new List<ISettingsElement>();

            active = false;
            rect = new RectangleF();
            white_rect = new RectangleF();
            font = Font.CourierNew24;

            TabButtons();
            GameButtons(game);
            UpdateLayout(graphics_device);

            SelectorElements(game, graphics_device, graphics);

            mouse_listener.MouseUp += (s, e) =>
            {
                foreach (var button in tab_list)
                {
                    if (button.Clicked(e))
                    {
                        tab_list.ForEach(b => b.is_selected = false);
                        button.is_selected = true;
                    }
                }
            };
            keyboard_listener.KeyReleased += (s, e) =>
            {
                if (e.Key == Keys.Escape) active = false;
            };
        }

        void TabButtons()
        {
            game_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), font, "Game");
            game_button.is_selected = true;

            sound_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), font, "Sound");
            graphics_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(40, 40, 40), font, "Graphics");
            
            close_button = new Button("Textures\\UI\\icon_01", Vector2.Zero, new Vector2(510, 510), new Color(215, 215, 215) * 0.5f, null, string.Empty);
            close_button.Size *= 0.04f;

            tab_list.Add(game_button);
            tab_list.Add(sound_button);
            tab_list.Add(graphics_button);

            foreach (var button in tab_list)
            {
                button.no_rectangle = true;
                button.Size = font.MeasureString(button.text);
            }

            close_button.on_active += () => active = false;
        }

        void GameButtons(Game game)
        {
            pause_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(0, 0, 0, 0f), font, "Pause");
            pause_button.no_rectangle = false;
            game_list.Add(pause_button);

            exit_button = new Button(null, Vector2.Zero, Vector2.Zero, new Color(0, 0, 0, 0f), font, "Exit");
            exit_button.no_rectangle = false;
            game_list.Add(exit_button);

            pause_button.on_active += () =>
            {
                active = false;
                MainMenu.active = true;
            };
            exit_button.on_active += game.Exit;
        }

        float vol = 0;
        bool graphics_changed = false;

        void SelectorElements(Game game, GraphicsDevice graphics_device, GraphicsDeviceManager graphics)
        {
            float y = white_rect.Height * 0.1f;

            volume_slider = new Slider("Volume", ref vol, white_rect, y);
            float offset_y = volume_slider.Size.Y + y;
            sound_list.Add(volume_slider);

            music_slider = new Slider("Music", ref vol, white_rect, y + offset_y);
            offset_y += music_slider.Size.Y + y;
            sound_list.Add(music_slider);

            sound_slider = new Slider("Sound", ref vol, white_rect, y + offset_y);
            sound_list.Add(sound_slider);

            GraphicsElements(game, graphics_device, graphics, y, offset_y);
        }

        void GraphicsElements(Game game, GraphicsDevice graphics_device, GraphicsDeviceManager graphics, float y, float offset_y)
        {
            selector = new Selector("Screen Mode", new string[] { "Windowed", "Fullscreen", "Borderless" }, () => GetScreenModeIndex(game, graphics), index => SetScreenMode(game, graphics_device, graphics, index), white_rect, y);
            graphics_list.Add(selector);
            offset_y = selector.Size.Y + y * 2f;

            vsync_box = new CheckBox("VSync", () => graphics.SynchronizeWithVerticalRetrace, value => { graphics.SynchronizeWithVerticalRetrace = value; graphics_changed = true; }, white_rect, y + offset_y);
            graphics_list.Add(vsync_box);

            apply_button = new Button(null, Vector2.Zero, new Vector2(150, 50), new Color(120, 120, 120), Font.CourierNew24, "Apply");
            apply_button.Position = new Vector2(white_rect.Center.X, white_rect.Y + white_rect.Height - apply_button.Size.Y);

            selector.on_changed += () => selector.UpdatePosition(white_rect);
            apply_button.on_active += () =>
            {
                if (graphics_changed)
                {
                    graphics.PreferredBackBufferWidth = game.GraphicsDevice.Viewport.Width;
                    graphics.PreferredBackBufferHeight = game.GraphicsDevice.Viewport.Height;
                    graphics.ApplyChanges();
                    graphics_changed = false;
                }
            };
        }

        public int GetScreenModeIndex(Game game, GraphicsDeviceManager graphics)
        {
            if (graphics.IsFullScreen)
                return 1;
            if (!game.Window.IsBorderless)
                return 0;
            return 2;
        }

        public void SetScreenMode(Game game, GraphicsDevice graphics_device, GraphicsDeviceManager graphics, int index)
        {
            var display_mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            switch (index)
            {
                case 0:
                    game.Window.IsBorderless = false;
                    graphics.IsFullScreen = false;

                    graphics.PreferredBackBufferWidth = 1280;
                    graphics.PreferredBackBufferHeight = 720;
                    graphics.ApplyChanges();

                    game.Window.Position = new Point(
                        (display_mode.Width - graphics.PreferredBackBufferWidth) / 2,
                        (display_mode.Height - graphics.PreferredBackBufferHeight) / 2
                    );

                    UpdateLayout(graphics_device);
                    break;
                case 1:
                    graphics.IsFullScreen = true;

                    graphics.ApplyChanges();
                    break;
                case 2:
                    graphics.IsFullScreen = false;
                    game.Window.IsBorderless = true;

                    graphics.PreferredBackBufferWidth = display_mode.Width;
                    graphics.PreferredBackBufferHeight = display_mode.Height;
                    graphics.ApplyChanges();

                    game.Window.Position = Point.Zero;

                    UpdateLayout(graphics_device);
                    break;
            }
        }

        public void UpdateLayout(GraphicsDevice graphics_device)
        {
            rect = graphics_device.Viewport.Bounds.ToRectangleF();
            float width = rect.Width * 0.48f;
            float height = rect.Height * 0.48f;
            float x = rect.X + rect.Width / 2f - width / 2f;
            float y = rect.Y + rect.Height / 2f - height / 2f;
            rect = new RectangleF(x, y, width, height);

            float white_height = rect.Height * 0.9f;
            white_rect = new RectangleF(rect.X, rect.Bottom - white_height, rect.Width, white_height - tab_list[0].Size.Y / 2f);

            float offset = rect.Width * 0.04f;
            float offset_x = rect.Width * 0.02f;
            float offset_y = (rect.Top + white_rect.Top) / 2f;
            close_button.Position = new Vector2(rect.Right - close_button.Size.X / 2f - offset_x, offset_y);

            for (int i = 0; i < tab_list.Count; i++)
            {
                tab_list[i].Position = new Vector2(rect.Left + tab_list[i].Size.X / 2f + offset_x, offset_y);
                offset_x += tab_list[i].Size.X + offset;
            }

            SizeF pause_size = font.MeasureString(pause_button.text);
            SizeF exit_size = font.MeasureString(exit_button.text);

            pause_button.Size = new Vector2(white_rect.Width * 0.98f, pause_size.Height * 2f);
            exit_button.Size = new Vector2(white_rect.Width * 0.98f, exit_size.Height * 2f);

            float center_x = white_rect.X + white_rect.Width / 2f;
            offset_y = white_rect.Height * 0.02f;
            pause_button.Position = new Vector2(center_x, white_rect.Top + pause_button.Size.Y / 2f + offset_y);
            exit_button.Position = new Vector2(center_x, pause_button.Position.Y + pause_button.Size.Y + offset_y);

            sound_list.ForEach(s => s.UpdatePosition(white_rect));
            graphics_list.ForEach(b => b.UpdatePosition(white_rect));

            if (apply_button != null) apply_button.Position = new Vector2(white_rect.Center.X, white_rect.Y + white_rect.Height - apply_button.Size.Y);
        }

        public void Update(GameTime game_time)
        {
            if (!active) return;
            mouse_listener.Update(game_time);
            keyboard_listener.Update(game_time);

            tab_list.ForEach(b => b.Update(game_time));
            close_button.Update(game_time);

            if (game_button.is_selected)
            {
                game_list.ForEach(b => b.Update(game_time));
            }
            else if (sound_button.is_selected)
            {
                sound_list.ForEach(s => s.Update(game_time));
            }
            else if (graphics_button.is_selected)
            {
                graphics_list.ForEach(b => b.Update(game_time));
                if (graphics_changed) apply_button.Update(game_time);
            }
        }
        
        public void Draw(SpriteBatch sprite_batch)
        {
            if (!active) return;
            sprite_batch.FillRectangle(rect, Color.Black * 0.75f);
            sprite_batch.DrawRectangle(white_rect, Color.White * 0.5f);
            tab_list.ForEach(b => b.Draw(sprite_batch));
            close_button.Draw(sprite_batch);
            
            if (game_button.is_selected)
            {
                game_list.ForEach(b => b.Draw(sprite_batch));
            }
            else if (sound_button.is_selected)
            {
                sound_list.ForEach(s => s.Draw(sprite_batch));
            }
            else if (graphics_button.is_selected)
            {
                graphics_list.ForEach(s => s.Draw(sprite_batch));
                if (graphics_changed) apply_button.Draw(sprite_batch);
            }
        }
    }
}
