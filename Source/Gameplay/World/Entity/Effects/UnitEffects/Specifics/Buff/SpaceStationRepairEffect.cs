using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace EndlessSpace
{
    public class SpaceStationRepairEffect : UnitEffect
    {
        float total_magnitude;

        public SpaceStationRepairEffect(Unit source) : base("Space Station Repair", source, null, source.GetBaseUnitValue(UnitValue.Health) * 0.1f, 6f, 0f)
        {
            total_magnitude = 0f;
            float base_magnitude = source.GetBaseUnitValue(UnitValue.Heal);
            source.SetBaseUnitValue(UnitValue.Heal, base_magnitude + magnitude * 0.01f);
            total_magnitude += magnitude * 0.1f;
            source.RestoreUnitValue(UnitValue.Health, magnitude);
            source.EffectTarget.ActivateThrob(Color.DeepSkyBlue);
        }

        public override void OnEffectEnd()
        {
            float base_magnitude = source.GetBaseUnitValue(UnitValue.Heal);
            source.SetBaseUnitValue(UnitValue.Heal, base_magnitude - total_magnitude);
            total_magnitude = 0f;
            base.OnEffectEnd();
        }
    }
}
