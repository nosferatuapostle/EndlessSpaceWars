using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class BomberHitModifier(Unit source, Weapon weapon) : HitEffect("Bomber Hit Modifier", source, weapon)
    {
        public override void Activate(Unit target)
        {
            EntityManager.PassProjectile(new BomberProj(target.Position, source, target));
        }
    }
}
