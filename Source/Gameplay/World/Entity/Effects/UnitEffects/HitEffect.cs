using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessSpace
{
    public abstract class HitEffect : UnitEffect
    {
        public HitEffect(string name, Unit source, Weapon weapon) : base(name, source, null, 0f, 0f, 0f)
        {
            var original_projectile = weapon.projectile;
            weapon.projectile = (owner, target) =>
            {
                var proj = original_projectile(owner, target);
                proj.on_hit += target_unit => Activate(target_unit);
                return proj;
            };
        }

        public abstract void Activate(Unit target);
    }
}
