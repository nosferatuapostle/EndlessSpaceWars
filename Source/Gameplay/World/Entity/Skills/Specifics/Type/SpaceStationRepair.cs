using System;

namespace EndlessSpace
{
    public class SpaceStationRepair(Unit owner) : Skill("Space Station Repair", Tag.Heal, owner, 2f)
    {
        protected override void Use()
        {
            owner.EffectTarget.AddEffect(new SpaceStationRepairEffect(owner));
        }
    }
}
