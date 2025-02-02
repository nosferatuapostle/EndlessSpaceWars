using System;
using System.Linq;

namespace EndlessSpace
{
    public class SupportHealCircle(Unit owner) : Skill("Support Heal Circle", Tag.PowerUp, owner, 2f)
    {
        protected override void Use()
        {
            EntityManager.PassProjectile(new SupportHealCircleProj(owner.Position, owner));
        }
    }
}
