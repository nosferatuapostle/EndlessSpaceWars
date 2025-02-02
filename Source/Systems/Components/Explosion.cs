using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles;

namespace EndlessSpace
{
    public class Explosion
    {
        public Vector2 Position { get; private set; }
        ParticleEffect particle_effect;
        float duration;

        public Explosion(Vector2 position, ParticleEffect effect, float duration)
        {
            Position = position;
            particle_effect = effect;
            this.duration = duration;
        }

        public bool IsComplited => duration <= 0;
        
        public void Update(GameTime game_time)
        {
            duration -= (float)game_time.ElapsedGameTime.TotalSeconds;
            particle_effect.Position = Position;
            particle_effect.Update((float)game_time.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(particle_effect);
        }
    }

}
