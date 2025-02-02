using System;
using System.Linq;

namespace EndlessSpace
{
    public class FrigateStoneArmorEffect : UnitEffect
    {
        float total_magnitude;

        public FrigateStoneArmorEffect(Unit source) : base("Frigate Stone Armor Effect", source, null, 1f, 5f, 0f)
        {
            total_magnitude = 0f;
            source.AddKeyword("stone_armor");
            float base_resist = source.GetBaseUnitValue(UnitValue.DamageResist);
            source.SetBaseUnitValue(UnitValue.DamageResist, base_resist + magnitude);
            total_magnitude += magnitude;
        }

        public override void OnEffectEnd()
        {
            source.RemoveKeyword("stone_armor");
            float base_resist = source.GetBaseUnitValue(UnitValue.DamageResist);
            source.SetBaseUnitValue(UnitValue.DamageResist, base_resist - total_magnitude);
            total_magnitude = 0f;
            base.OnEffectEnd();
        }
    }
}
