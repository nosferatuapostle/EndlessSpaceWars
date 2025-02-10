using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace EndlessSpace
{
    public class UserInterface
    {
        GraphicsDevice graphics_device;

        MainMenu main_menu;
        SettingsMenu settings_menu;

        StatsMenu stats_menu;

        LevelBar level_bar;
        LevelInfo level_info;

        SkillBar skill_bar;

        public UserInterface(Game game, GraphicsDevice graphics_device, GraphicsDeviceManager graphics, PlayerCharacter player)
        {
            this.graphics_device = graphics_device;

            main_menu = new MainMenu(graphics_device);
            settings_menu = new SettingsMenu(game, graphics_device, graphics);

            stats_menu = new StatsMenu(player, graphics_device);

            level_bar = new LevelBar(player);
            level_info = new LevelInfo(player);

            skill_bar = new SkillBar(player, graphics_device);

            game.Window.ClientSizeChanged += (s, e) =>
            {
                main_menu.UpdateLayout(graphics_device);
                settings_menu.UpdateLayout(graphics_device);
                stats_menu.UpdateLayout(graphics_device);
                skill_bar.UpdateLayout(graphics_device);
            };
        }

        public void Update(GameTime game_time)
        {
            main_menu.Update(game_time);
            settings_menu.Update(game_time);

            if (MainMenu.active || SettingsMenu.active) return;
            stats_menu.Update(game_time);

            level_bar.Update(graphics_device, game_time.GetElapsedSeconds());
            level_info.Update(game_time, level_bar);

            skill_bar.Update(game_time);
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            main_menu.Draw(sprite_batch);
            settings_menu.Draw(sprite_batch);

            if (MainMenu.active || SettingsMenu.active) return;
            stats_menu.Draw(sprite_batch);

            level_bar.Draw(sprite_batch);
            level_info.Draw(sprite_batch);

            skill_bar.Draw(sprite_batch);
        }
    }
}
