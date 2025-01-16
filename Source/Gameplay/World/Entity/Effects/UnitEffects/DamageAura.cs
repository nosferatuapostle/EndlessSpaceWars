using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles;
using MonoGame.Extended;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    public class DamageAura : UnitEffect
    {
        float radius;
        Color color;

        public DamageAura(string name, float radius, Unit source, Unit target, float magnitude) : base(name, source, target, magnitude, 0f, 0.4f)
        {
            this.radius = radius;

            color = Color.Red;

            particle_effect = new ParticleEffect
            {
                Position = target.Position,
                Emitters =
                {
                    new ParticleEmitter(new Texture2DRegion(Particle.Spark), 20, TimeSpan.FromSeconds(1.5), Profile.Circle(10f, Profile.CircleRadiation.Out))
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
            if (source.IsDead || target.IsDead || (target is Character character && !character.HostileTo(source)) || Vector2.Distance(source.Position, target.Position) > radius || target.Faction == UnitFaction.Summoned) IsDone = true;
            
            base.Update(game_time);
        }

        protected override void ApplyEffect()
        {
            target.GetDamage(source, base_magnitude, color);
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(particle_effect);
        }
    }
}
