using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    public class LevelBar : ProgressBar
    {
        PlayerCharacter player;

        public LevelBar(PlayerCharacter player) : base(Vector2.Zero, Vector2.Zero)
        {
            this.player = player;

            color = Color.Goldenrod;
        }

        public virtual void Update(GraphicsDevice graphics_device, float dtime)
        {            
            position = new Vector2((int)(graphics_device.Viewport.Width / 2f - size.X / 2), (int)(graphics_device.Viewport.Height * 0.05f - size.Y / 2));
            size = new Vector2(graphics_device.Viewport.Width * 0.4f, 4f);
            
            base.Update(player.Experience.NextXP, player.Experience.CurrentXP, dtime);
        }
    }
}
