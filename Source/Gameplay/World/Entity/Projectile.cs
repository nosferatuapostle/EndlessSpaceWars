using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Timers;
using System;

namespace EndlessSpace
{
    public class Projectile : AnimatedObject, ICollisionActor
    {
        public event Action<Unit> on_hit;

        bool is_done;
        protected float speed, damage;

        Vector2 direction;

        protected Unit owner;
        object target;

        protected CountdownTimer life_time;

        public Projectile(string[] path, Vector2 position, Vector2 size, Unit owner, object target) : base(path, position, size)
        {
            is_done = false;
            speed = 0f;
            damage = 0f;
            this.owner = owner;
            this.target = target;
            life_time = new CountdownTimer(25f);

            if (target == null) return;
            direction = direction = Vector2.Normalize(GetTarget - Position);
            Rotation = Position.ToAngle(GetTarget);
        }

        public bool IsDone { get { return is_done; } }

        public string Name { get; set; } = "None";
        public Vector2 GetTarget { get { return target is Unit unit ? unit.Position : (Vector2)target; } }

        public float Range { get; protected set; } = 100f;
        public CountdownTimer Cooldown { get; protected set; } = new CountdownTimer(1f);

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Unit unit && unit != owner && unit == target && !unit.IsDead)
            {
                float radius = unit.Faction == UnitFaction.Summoned ? unit.Size.X : unit.Size.X / 4f;
                float distance = Vector2.Distance(Position, unit.Position);

                if (distance > radius) return;
                OnHit(unit);
            }
        }

        protected virtual void OnHit(Unit target)
        {
            Bounds = new CircleF(Vector2.Zero, 0);
            OnHit(target, Color.Red);
            is_done = true;
        }

        protected virtual void OnHit(Unit target, Color color)
        {
            on_hit?.Invoke(target);
            target.GetDamage(owner, damage, color);
        }

        public override void Update(GameTime game_time)
        {
            if (target is Unit unit && !unit.IsDead)
            {
                direction = Vector2.Normalize(GetTarget - Position);
                Rotation = Position.ToAngle(GetTarget);
                ProjPosition(game_time);
            }
            else
            {
                ProjPosition(game_time);
            }

            life_time.Update(game_time);
            if (life_time.State == TimerState.Completed)
            {
                is_done = true;
            }

            Bounds.Position = Position;
            base.Update(game_time);
        }

        public virtual void ProjPosition(GameTime game_time)
        {
            Position += direction * speed * game_time.GetElapsedSeconds();
        }

        public virtual ParticleEffect Explosion()
        {
            ParticleEffect explosion_effect = new ParticleEffect
            {
                Position = Position,
                Emitters = 
                {
                    new ParticleEmitter(new Texture2DRegion(Particle.Spark), 5, TimeSpan.FromSeconds(1.5), Profile.Circle(10f, Profile.CircleRadiation.Out))
                    {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(0f, 50f),
                        Quantity = 1,
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(0.5f, 1.0f)
                        }
                    }
                }
            };

            return explosion_effect;
        }
    }
}
