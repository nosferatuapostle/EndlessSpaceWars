using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class FloatingDamage
    {
        string text;
        Vector2 position;
        Color color;
        CountdownTimer timer;

        public FloatingDamage(string text, Vector2 position, Color color)
        {
            this.text = text;
            this.position = position;
            this.color = color;

            timer = new CountdownTimer(1f);
        }

        public bool Update(GameTime game_time)
        {
            timer.Update(game_time);
            position.Y -= 10f * game_time.GetElapsedSeconds();
            return timer.State == TimerState.Completed;
        }

        public void Draw(SpriteBatch sprite_batch, BitmapFont font) => sprite_batch.DrawString(font, text, position, color);
    }
}
