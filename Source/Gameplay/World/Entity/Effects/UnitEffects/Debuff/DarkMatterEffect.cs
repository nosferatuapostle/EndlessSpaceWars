using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class DarkMatterEffect(Unit source, Unit target) : UnitEffect("Desiccation", source, target, 0.2f, 5f)
    {
        protected override void ApplyEffect()
        {
            target.GetDamage(source, base_magnitude, Color.DarkSlateBlue);
        }
    }
}
