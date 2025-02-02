namespace EndlessSpace
{
    public class Heal(Unit owner) : Skill("Heal", Tag.Heal, owner, 2f)
    {
        protected override void Use()
        {
            owner.EffectTarget.AddEffect(new HealEffect(owner));
        }
    }
}
