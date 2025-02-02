using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles;
using MonoGame.Extended;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    public class BuffAura : UnitEffect
    {
        Color color;

        UnitValue[] values;
        float[] value_mult_list;

        float[] value_increments;

        public BuffAura(string name, Unit source, Unit target, UnitValue[] values, float[] value_mult_list, float magnitude) : base(name, source, target, magnitude, 1.2f, 0f)
        {
            this.values = values;
            this.value_mult_list = value_mult_list;

            value_increments = new float[values.Length];

            color = Color.Plum;

            for (int i = 0; i < values.Length; i++)
            {
                float current_value = target.GetBaseUnitValue(values[i]);
                value_increments[i] = value_mult_list[i] * base_magnitude;
                target.SetBaseUnitValue(values[i], current_value + value_increments[i]);
            }

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
            base.Update(game_time);
        }

        public override void OnEffectEnd()
        {
            for (int i = 0; i < values.Length; i++)
            {
                float current_value = target.GetBaseUnitValue(values[i]);
                target.SetBaseUnitValue(values[i], current_value - value_increments[i]);
            }
            base.OnEffectEnd();
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(particle_effect);
        }
    }
}
