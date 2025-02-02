using System;
using System.Linq;

namespace EndlessSpace
{
    public class FighterExtraDamageEffect(Unit source, Weapon weapon) : HitEffect("Fighter Extra Damage", source, weapon, 10f)
    {
        public override void Activate(Unit target)
        {
            float max_health = target.GetBaseUnitValue(UnitValue.Health);
            float current_health = target.GetUnitValue(UnitValue.Health);

            float missing_health = (max_health - current_health) / max_health;
            float damage = (weapon.Damage * missing_health) / 2f;
            target.GetDamage(source, damage);
        }
    }
}
