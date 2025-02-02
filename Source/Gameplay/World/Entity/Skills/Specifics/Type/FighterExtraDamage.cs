using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace EndlessSpace
{
    public class FighterExtraDamage(Unit owner, Weapon weapon) : Skill("Fighter Extra Damage", Tag.Attack, owner, 2f)
    {
        protected override void Use()
        {
            owner.EffectTarget.AddEffect(new FighterExtraDamageEffect(owner, weapon));
        }
    }
}
