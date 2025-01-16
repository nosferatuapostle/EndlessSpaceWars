using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class InvisProj : Projectile
    {
        protected bool periodic;
        protected CountdownTimer tick_time;

        public InvisProj(Vector2 position, Vector2 size, Unit owner, object target) : base(null, position, size, owner, target)
        {
            periodic = false;
            tick_time = new CountdownTimer(0.1f);

            Bounds = new CircleF(position, size.X / 2.5f);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (tick_time.State == TimerState.Completed)
            {
                if (periodic)
                {
                    if (collisionInfo.Other is Unit unit && unit != owner)
                    {
                        tick_time.Restart();
                        unit.GetDamage(owner, damage);
                    }
                }
                else
                {
                    base.OnCollision(collisionInfo);
                }
            }
        }

        public override void Update(GameTime game_time)
        {
            tick_time.Update(game_time);
            base.Update(game_time);
        }
    }
}
