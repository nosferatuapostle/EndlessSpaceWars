using System;
using System.Linq;

namespace EndlessSpace
{
    public class BiomantesInvisability(Unit owner) : Skill("Biomantes Invisability", Tag.Escape, owner, 1f)
    {
        protected override void Use()
        {
            owner.EffectTarget.AddEffect(new BiomantesInvisabilityEffect(owner));
        }
    }
}
