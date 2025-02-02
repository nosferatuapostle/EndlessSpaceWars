using System;
using System.Linq;

namespace EndlessSpace
{
    public class BomberBomb(Unit owner) : Skill("Bomber Bomb", Tag.Escape, owner, 2f)
    {
        protected override void Use()
        {
            EntityManager.PassProjectile(new BomberBombProj(owner.Position, owner));
        }
    }
}
