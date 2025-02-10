using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    public interface ILightning
    {
        bool IsComplete { get; }

        void Update(GameTime game_time);
        void Draw(SpriteBatch sprite_batch);
    }
}
