using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Heal(Unit owner) : Skill("Heal", owner)
    {
        protected override void Use()
        {
            owner.EffectTarget.AddEffect(new HealEffect(owner, owner));
        }
    }
}
