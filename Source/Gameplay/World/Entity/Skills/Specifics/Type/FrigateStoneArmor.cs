using System;
using System.Linq;

namespace EndlessSpace
{
    public class FrigateStoneArmor : Skill
    {
        public FrigateStoneArmor(Unit owner) : base("FrigateStoneArmor", Tag.PowerUp, owner, 2f) { }

        protected override void Use()
        {
            owner.EffectTarget.AddEffect(new FrigateStoneArmorEffect(owner));
        }
    }
}
