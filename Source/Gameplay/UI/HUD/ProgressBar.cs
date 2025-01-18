using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    public class ProgressBar
    {
        public Vector2 position, size;

        protected BasicObject foreground, background;
        protected Color color;

        public ProgressBar(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
            color = Color.White;

            foreground = new BasicObject("Textures\\UI\\ProgressBarWhite", Vector2.Zero, size);
            background = new BasicObject("Textures\\UI\\ProgressBarBKG", Vector2.Zero, size);
        }

        public virtual void Update(float instance, float current, float delta_time)
        {
            background.Size = size;
            foreground.Size = new Vector2(current/ instance * size.X, size.Y);
        }

        public virtual void Draw(SpriteBatch sprite_batch)
        {
            background.Draw(sprite_batch, position, Vector2.Zero, Color.White);
            foreground.Draw(sprite_batch, position, Vector2.Zero, color);
        }
    }
}
