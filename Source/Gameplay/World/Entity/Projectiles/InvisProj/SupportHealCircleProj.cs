using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Profiles;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    public class SupportHealCircleProj : InvisProj
    {
        ParticleEffect particle_effect;
        List<Unit> affected_units = new List<Unit>();

        public SupportHealCircleProj(Vector2 position, Unit owner) : base(position, new Vector2(400, 400), owner, null, 3f)
        {
            Name = "Support Heal Circle";
            damage = 0.5f;
            speed = 0f;

            particle_effect = new ParticleEffect()
            {
                Position = Position,
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(new Texture2DRegion(Particle.Point), 500, TimeSpan.FromSeconds(1), Profile.Ring(radius, Profile.CircleRadiation.In))
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 50f),
                            Quantity = 3,
                            Rotation = new Range<float>(-1f, 1f),
                            Scale = new Range<float>(1.0f, 3.0f)
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = new HslColor(0.33f, 0.5f, 0.5f),
                                        EndValue = new HslColor(0.5f, 0.9f, 1.0f)
                                    }
                                }
                            },
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
                        }
                    }
                }
            };
        }

        protected override bool HitCondition(Unit unit) => !unit.IsDead && !unit.HostileTo(owner);

        protected override void HitAction(Unit target)
        {
            if (affected_units.Contains(target)) return;
            target.EffectTarget.AddEffect(new SupportHealBuffEffect(owner, target, affected_units, damage, time + 1f));
        }

        public override void Update(GameTime game_time)
        {
            particle_effect.Update(game_time.GetElapsedSeconds());
            base.Update(game_time);
        }

        public override void Draw(SpriteBatch sprite_batch) => sprite_batch.Draw(particle_effect);
    }
}
