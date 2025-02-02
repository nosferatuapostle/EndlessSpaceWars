using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles;
using MonoGame.Extended;
using System;

namespace EndlessSpace
{
    public class DamageAura : UnitEffect
    {
        public DamageAura(string name, Unit source, Unit target, float magnitude, Color color) : base(name, source, target, magnitude, 1.2f, 0.4f)
        {
            particle_effect = new ParticleEffect
            {
                Position = target.Position,
                Emitters =
                {
                    new ParticleEmitter(new Texture2DRegion(Particle.Spark), 20, TimeSpan.FromSeconds(1), Profile.Circle(10f, Profile.CircleRadiation.Out))
                    {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(0f, 50f),
                        Quantity = 2,
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(1f, 2f),
                        Color = color.ToHsl()
                        }
                    }
                }
            };
        }

        public override void Update(GameTime game_time)
        {
            particle_effect.Position = target.Position;
            
            base.Update(game_time);
        }

        protected override void ApplyEffect()
        {
            target.GetDamage(source, base_magnitude);
        }
    }
}
