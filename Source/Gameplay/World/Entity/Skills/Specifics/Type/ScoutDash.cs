using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace EndlessSpace
{
    public class ScoutDash(Unit owner) : Skill("Scout Dash", Tag.Escape, owner, 2f)
    {
        const float DISTANCE = 160f, DURATION = 0.6f;

        bool is_dashing = false;
        float dash_time = 0f;
        Vector2 direction, position;

        protected override void Use()
        {
            if (is_dashing) return;

            direction = new Vector2(
                (float)Math.Cos(owner.Rotation - MathHelper.PiOver2),
                (float)Math.Sin(owner.Rotation - MathHelper.PiOver2)
            );

            direction.Normalize();

            position = owner.Position;

            is_dashing = true;
            dash_time = 0f;

            owner.EffectTarget.AddEffect(new ScoutDashEffect(owner, DURATION * 2f));
        }

        public override void Update(GameTime game_time)
        {
            base.Update(game_time);

            if (is_dashing)
            {
                dash_time += game_time.GetElapsedSeconds();

                float progress = MathHelper.Clamp(dash_time / DURATION, 0f, 1f);

                owner.Position = position + direction * DISTANCE * progress;

                if (dash_time >= DURATION)
                {
                    is_dashing = false;
                }
            }
        }
    }
}