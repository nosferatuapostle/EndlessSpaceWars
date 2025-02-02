using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles.Modifiers;

namespace EndlessSpace
{
    public class SupportHealBuffEffect : UnitEffect
    {
        Dictionary<Unit, float> affected_units = new Dictionary<Unit, float>();

        public SupportHealBuffEffect(Unit source, Unit target, List<Unit> unit_list, float magnitude, float duration) : base("Support Heal Buff", source, target, magnitude, duration, 0f)
        {
            target.SetBaseUnitValue(UnitValue.Heal, target.GetBaseUnitValue(UnitValue.Heal) + magnitude);
            affected_units[target] = magnitude;
            unit_list.Add(target);

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
                            Color = Color.Blue.ToHsl()
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    /*new ColorInterpolator
                                    {
                                        StartValue = new HslColor(0.33f, 0.5f, 0.5f),
                                        EndValue = new HslColor(0.5f, 0.9f, 1.0f)
                                    }*/
                                }
                            },
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
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
            foreach (var (unit, magnitude) in affected_units)
            {
                unit.SetBaseUnitValue(UnitValue.Heal, unit.GetBaseUnitValue(UnitValue.Heal) - magnitude);
            }
            affected_units.Clear();
            base.OnEffectEnd();
        }
    }
}
