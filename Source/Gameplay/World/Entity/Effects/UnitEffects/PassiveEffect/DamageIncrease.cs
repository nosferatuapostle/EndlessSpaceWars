
namespace EndlessSpace
{
    public class DamageIncrease(Unit source) : UnitEffect("DamageIncrease", source, null, 1f)
    {
        protected override void ApplyEffect()
        {
            source.SetBaseUnitValue(UnitValue.Magnitude, magnitude);
        }
    }
}
