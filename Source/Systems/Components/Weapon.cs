using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class Weapon
    {
        Unit owner;

        public Func<Unit, object, Projectile> projectile;
        readonly Projectile prototype;
        
        CountdownTimer cooldown_time;

        List<Unit> unit_list;

        public Weapon(Unit owner, Func<Unit, object, Projectile> projectile)
        {
            this.owner = owner;
            this.projectile = projectile;

            prototype = projectile(owner, null);
            cooldown_time = prototype.Cooldown;
        }

        public Unit Target { get; private set; } = null;

        public float Range => prototype.Range;
        public CountdownTimer Cooldown => prototype.Cooldown;

        public void Update(GameTime game_time)
        {
            Cooldown.Update(game_time);
        }

        public void PassProjectile(Unit target, float dtime)
        {
            if (target == null || target.IsDead)
            {
                Target = null;
                Cooldown.Restart();
                return;
            }

            Target = target;
            

            float distance = (target.Position - owner.Position).Length();

            if (Cooldown.State == TimerState.Completed)
            {
                if (distance < Range)
                {
                    float angle = Math.Abs(MathHelper.WrapAngle(owner.Rotation - owner.Position.ToAngle(target.Position)));
                    if (angle < 0.1f)
                    {
                        Projectile projectile = this.projectile(owner, target);
                        owner.Event.OnAttack(owner, target);
                        EntityManager.PassProjectile(projectile);
                        Cooldown.Restart();
                    }
                }
            }

            owner.Rotation = owner.Rotate(owner.Rotation, owner.Position.ToAngle(target.Position), dtime);
        }
    }
}
