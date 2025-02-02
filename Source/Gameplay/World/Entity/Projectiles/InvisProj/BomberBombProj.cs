using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class BomberBombProj : InvisProj
    {
        HashSet<Unit> affected_units;

        public BomberBombProj(Vector2 position, Unit owner) : base(position, owner.Size * 2f, owner, null, 15f)
        {
            Name = "Bomber Bomb";
            damage = 4f;
            speed = 0f;

            affected_units = new HashSet<Unit>();
        }

        protected override void HitAction(Unit target)
        {
            if (affected_units.Contains(target)) return;
            affected_units.Add(target);
            target.GetDamage(owner, damage, Color.Red);
            life_time = new CountdownTimer(0f);
        }

        public override ParticleEffect Explosion()
        {
            var particleEffect = new ParticleEffect()
            {
                Emitters = new List<ParticleEmitter>
    {
        new ParticleEmitter(new Texture2DRegion(Particle.Spark), 500, TimeSpan.FromSeconds(2.5),
            Profile.Circle(Size.X/2f, Profile.CircleRadiation.Out))
        {
            Parameters = new ParticleReleaseParameters
            {
                Speed = new Range<float>(0f, 50f),
                Quantity = 3,
                Rotation = new Range<float>(-1f, 1f),
                Scale = new Range<float>(1.0f, 1.0f)
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
                new RotationModifier {RotationRate = -2.1f},
                new RectangleContainerModifier {Width = 800, Height = 480},
                new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
            }
        }
    }
            };

            return particleEffect;
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.DrawRectangle(GetRectangle(), Color.White);
            base.Draw(sprite_batch);
        }
    }
}
