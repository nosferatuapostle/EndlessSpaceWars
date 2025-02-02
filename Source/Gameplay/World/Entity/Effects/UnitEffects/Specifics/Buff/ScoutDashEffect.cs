using MonoGame.Extended.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Profiles;
using System;
using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class ScoutDashEffect : UnitEffect
    {
        public ScoutDashEffect(Unit source, float duration) : base("Scout Dash Effect", source, null, 0f, duration, 0f)
        {
            particle_effect = new ParticleEffect
            {
                Position = source.Position,
                Emitters =
                {
                    new ParticleEmitter(new Texture2DRegion(Particle.Spark), 100, TimeSpan.FromSeconds(1), Profile.Circle(10f, Profile.CircleRadiation.Out))
                    {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(0f, 20f),
                        Quantity = 2,
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(1f, 2f),
                        Color = Color.AliceBlue.ToHsl()
                        }
                    }
                }
            };
        }

        public override void Update(GameTime game_time)
        {
            particle_effect.Position = source.Position;
            base.Update(game_time);
        }
    }
}
