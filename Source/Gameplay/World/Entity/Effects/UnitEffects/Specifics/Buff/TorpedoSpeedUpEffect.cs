using System;
using System.Linq;

namespace EndlessSpace
{
    public class TorpedoSpeedUpEffect : UnitEffect
    {
        float total_magnitude;

        public TorpedoSpeedUpEffect(Unit source) : base("Torpedo Speed Up", source, null, 0.5f, 5f, 0f)
        {
            total_magnitude = 0f;
            source.SetBaseUnitValue(UnitValue.SpeedMult, source.GetUnitValue(UnitValue.SpeedMult) + magnitude);
            total_magnitude += magnitude;
        }

        public override void OnEffectEnd()
        {
            source.SetBaseUnitValue(UnitValue.SpeedMult, source.GetUnitValue(UnitValue.SpeedMult) - total_magnitude);
            total_magnitude = 0f;
            base.OnEffectEnd();
        }
    }
}
