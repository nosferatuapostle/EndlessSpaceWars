using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace EndlessSpace
{
    public class Movable
    {
        const float MIN_LENGTH = 10f;

        Unit unit;
        Vector2 direction, target;
        bool is_moving;

        public Movable(Unit unit)
        {
            this.unit = unit;
            direction = Vector2.Zero;
            target = unit.Position;
            is_moving = false;
        }

        public void SetTarget(Vector2 target)
        {
            this.target = target;
            is_moving = true;
        }

        public void Stop() => is_moving = false;

        public void Update(GameTime game_time)
        {
            if (!is_moving || unit.IsDead) return;
            {
                direction = target - unit.Position;

                if (direction.Length() > MIN_LENGTH)
                {
                    direction.Normalize();
                    unit.Velocity = direction * unit.GetUnitValue(UnitValue.SpeedMult);
                    unit.Position += unit.Velocity * game_time.GetElapsedSeconds();
                }
                else
                {
                    Stop();
                }

                unit.Rotation = unit.Rotate(unit.Rotation, unit.Position.ToAngle(target), game_time.GetElapsedSeconds());
            }
        }
    }
}
