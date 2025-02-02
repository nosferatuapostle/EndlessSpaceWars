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
        public event Action on_destroy;

        protected bool is_done;
        protected float radius, speed, damage;

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
            life_time = new CountdownTimer(18f);

            Vector2 scaled_size = Size * Scale;
            radius = MathF.Sqrt(scaled_size.X * scaled_size.X + scaled_size.Y * scaled_size.Y) / 4f;

            if (target == null) return;
            direction = direction = Vector2.Normalize((target as Unit).Position - Position);
            Rotation = Position.ToAngle(GetTarget);
        }

        public bool IsDone { get { return is_done; } }
        public float Radius { get { return radius; } }

        public string Name { get; set; } = "None";
        public Vector2 GetTarget { get { return target is Unit unit ? unit.Position : (Vector2)target; } }

        public float Damage { get { return damage; } }
        public float Range { get; protected set; } = 100f;
        public CountdownTimer Cooldown { get; protected set; } = new CountdownTimer(1f);

        public void OnDestroy() => on_destroy?.Invoke();

        public virtual void OnCollision(CollisionEventArgs collision_info)
        {
            if (collision_info.Other is not Unit unit || !HitCondition(unit)) return;
            if (unit.Faction == UnitFaction.Summoned) return;
            if (unit.HasKeyword("space_station"))
            {
                RectangleF base_rect = unit.GetRectangle();
                base_rect.Inflate(-base_rect.Width * 0.2f, -base_rect.Height * 0.2f);

                if (!base_rect.Intersects(new RectangleF(Position.X - radius, Position.Y - radius, radius * 2, radius * 2)))
                    return;
            }
            else
            {
                float total_radius = (unit.Radius / 2f + radius) * (unit.Radius / 2f + radius);
                float distance = Vector2.DistanceSquared(Position, unit.Position);
                if (distance > total_radius) return;
            }
            OnHit(unit);
        }

        protected virtual bool HitCondition(Unit unit) => unit != owner && unit == target && !unit.IsDead;

        protected virtual void OnHit(Unit target)
        {
            on_hit?.Invoke(target);
            HitAction(target);
        }

        protected virtual void HitAction(Unit target)
        {
            Bounds = new CircleF(Vector2.Zero, 0);
            target.GetDamage(owner, damage, Color.Red);
            is_done = true;
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
