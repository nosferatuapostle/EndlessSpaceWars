using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace EndlessSpace
{
    public class Movable
    {
        const float MIN_LENGTH = 20f;
        const float BASE_SPEED = 100f;

        bool is_moving;

        Unit unit;
        Vector2 direction, target, acceleration, vel;

        public Movable(Unit unit)
        {
            is_moving = false;

            this.unit = unit;
            target = unit.Position;
            direction = Vector2.Zero;
            acceleration = Vector2.Zero;
            vel = Vector2.Zero;
        }

        public bool IsMovable => is_moving;

        public void SetTarget(Vector2 target)
        {
            this.target = target;
            is_moving = true;
        }

        public void Stop()
        {
            vel = Vector2.Zero;
            is_moving = false;
        }

        public void Update(GameTime game_time)
        {
            if (!is_moving || unit.IsDead) return;

            direction = target - unit.Position;

            if (direction.Length() > MIN_LENGTH)
            {
                direction.Normalize();
                Vector2 desired_velocity = direction * BASE_SPEED * unit.GetUnitValue(UnitValue.SpeedMult);

                acceleration = (desired_velocity - vel) * 2f;

                float delta_time = game_time.GetElapsedSeconds();

                vel += acceleration * delta_time;
                unit.Position += vel * delta_time;

                unit.Rotate(target, delta_time);
            }
            else
            {
                Stop();
            }
        }
    }
}
