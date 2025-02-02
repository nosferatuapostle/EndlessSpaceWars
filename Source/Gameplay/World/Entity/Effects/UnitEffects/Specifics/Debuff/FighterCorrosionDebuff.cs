using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class FighterCorrosionDebuff : UnitEffect
    {
        float total_debuff;

        public FighterCorrosionDebuff(Unit source, Unit target) : base("Fighter Corrosion Effect", source, target, 0.1f, 5f, 0f)
        {
            total_debuff = 0f;

            target.SetBaseUnitValue(UnitValue.DamageResist, target.GetBaseUnitValue(UnitValue.DamageResist) - magnitude);
            total_debuff += magnitude;
        }

        public override void OnEffectEnd()
        {
            base.OnEffectEnd();
            target.SetBaseUnitValue(UnitValue.DamageResist, target.GetBaseUnitValue(UnitValue.DamageResist) + total_debuff);
        }
    }
}
