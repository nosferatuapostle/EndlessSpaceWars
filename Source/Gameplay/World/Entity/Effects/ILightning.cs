using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    public interface ILightning
    {
        bool IsComplete { get; }

        void Update();
        void Draw(SpriteBatch sprite_batch);
    }
}
