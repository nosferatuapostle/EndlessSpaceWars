using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class HealEffect : UnitEffect
    {
        public HealEffect(Unit source) : base("HealEffect", source, null, 2f, 0f, 0f)
        {
            source.RestoreUnitValue(UnitValue.Health, magnitude);
            source.EffectTarget.ActivateThrob(Color.LightYellow);
            IsDone = true;
        }
    }
}
