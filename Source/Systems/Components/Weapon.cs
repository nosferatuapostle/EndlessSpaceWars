using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System;

namespace EndlessSpace
{
    public class Weapon
    {
        Unit owner;

        public Func<Unit, object, Projectile> projectile;
        Projectile prototype;

        public Weapon(Unit owner, Func<Unit, object, Projectile> projectile)
        {
            this.owner = owner;
            this.projectile = projectile;

            prototype = projectile(owner, null);
        }

        public Unit Target { get; private set; } = null;

        public float Damage => prototype.Damage;
        public float Range => prototype.Range;
        public CountdownTimer Cooldown => prototype.Cooldown;

        public void Update(GameTime game_time)
        {
            Cooldown.Update(game_time);
        }

        public void SwitchProjectile(Func<Unit, object, Projectile> projectile)
        {
            this.projectile = projectile;
            prototype = projectile(owner, null);
        }

        public void PassProjectile(Unit target, float delta_time)
        {
            Target = target;
            if (Cooldown.State == TimerState.Completed)
            {
                float angle = MathF.Abs(MathHelper.WrapAngle(owner.Rotation - owner.Position.ToAngle(target.Position)));
                if (angle < 0.1f)
                {
                    Projectile projectile = this.projectile(owner, target);
                    owner.Event.OnAttack(owner, target);
                    EntityManager.PassProjectile(projectile);
                    Cooldown.Restart();
                }
            }
        }
    }
}
