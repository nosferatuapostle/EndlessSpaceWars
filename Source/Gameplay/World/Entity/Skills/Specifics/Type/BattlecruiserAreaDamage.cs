using System;
using System.Linq;

namespace EndlessSpace
{
    public class BattlecruiserAreaDamage(Unit owner) : Skill("Battlecruiser Area Damage", Tag.Attack, owner, 2f)
    {
        protected override void Use()
        {
            EntityManager.PassProjectile(new BattlecruiserAreaDamageProj(owner.Position, owner));
        }
    }
}
