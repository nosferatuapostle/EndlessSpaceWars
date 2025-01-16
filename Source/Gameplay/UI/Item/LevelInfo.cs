using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace EndlessSpace
{
    public class LevelInfo
    {
        PlayerCharacter player;

        Vector2 position;
        string text;
        BitmapFont font;

        public LevelInfo(PlayerCharacter player)
        {
            this.player = player;

            font = Globals.Content.Load<BitmapFont>("Fonts/CourierNew16");
        }

        public virtual void Update(GameTime game_time, LevelBar level_bar)
        {
            text = $"{player.Experience.CurrentXP}/{player.Experience.NextXP} XP";
            SizeF text_size = font.MeasureString(text);

            position = new Vector2(level_bar.position.X + level_bar.size.X / 2f - text_size.Width / 2f, level_bar.position.Y + level_bar.size.Y + 10f);
        }

        public virtual void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.DrawString(font, text, position, Color.White);
        }
    }
}
