using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles;
using System;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using System.Collections.Generic;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class VortexProj : InvisProj
    {
        ParticleEffect particle_effect;

        public VortexProj(Vector2 pos, Unit owner, Unit target): base(pos, new Vector2(96, 96), owner, target)
        {
            Name = "TestParticles";
            damage = 0f;
            speed = 500f;
            periodic = true;
            tick_time = new CountdownTimer(0.2f);

            particle_effect = new ParticleEffect()
            {
                Position = Position,
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(new Texture2DRegion(Particle.Spark), 200, TimeSpan.FromSeconds(2.5), Profile.Circle(Width/2f, Profile.CircleRadiation.Out))
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 50f),
                            Quantity = 1,
                            Rotation = new Range<float>(-1f, 1f),
                            Scale = new Range<float>(1.0f, 2.0f)
                        },
                        Modifiers =
                        {
                            new RotationModifier {RotationRate = -2.1f},
                            new RectangleContainerModifier {Width = 800, Height = 480},
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
                        }
                    }
                }
            };
        }

        public override void Update(GameTime game_time)
        {
            particle_effect.Position = Position;
            particle_effect.Update(game_time.GetElapsedSeconds());            

            base.Update(game_time);
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(particle_effect);
            base.Draw(sprite_batch);
        }
    }
}
