using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class BomberHitModifier(Unit source, Weapon weapon) : HitEffect("Bomber Hit Modifier", source, weapon)
    {
        protected override void Awake()
        {
            AddKeyword("specific");
        }

        public override void Activate(Unit target)
        {
            EntityManager.PassProjectile(new BomberProj(target.Position, source, target));
        }
    }
}
