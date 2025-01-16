using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace EndlessSpace
{
    public class UserInterface
    {
        LevelBar level_bar;
        LevelInfo level_info;

        public UserInterface(PlayerCharacter player)
        {
            level_bar = new LevelBar(player);
            level_info = new LevelInfo(player);
        }

        public void Update(GraphicsDevice graphics_device, GameTime game_time)
        {
            level_bar.Update(graphics_device, game_time.GetElapsedSeconds());
            level_info.Update(game_time, level_bar);
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            level_bar.Draw(sprite_batch);
            level_info.Draw(sprite_batch);
        }
    }
}
