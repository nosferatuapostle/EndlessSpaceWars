using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class HealEffect(Unit source, Unit target) : UnitEffect("HealEffect", source, target, 4f, 1f, 0)
    {
        protected override void ApplyEffect()
        {
            target.RestoreUnitValue(UnitValue.Health, magnitude);
            target.EffectTarget.ActivateThrob(Color.LightYellow);
        }

        public override void OnEffectEnd()
        {
            target.EffectTarget.ActivateThrob(Color.LightYellow);
        }
    }
}
