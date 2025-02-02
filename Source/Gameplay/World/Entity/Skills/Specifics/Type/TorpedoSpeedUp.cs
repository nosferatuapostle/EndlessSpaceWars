using System;
using System.Linq;

namespace EndlessSpace
{
    public class TorpedoSpeedUp(Unit owner) : Skill("Torpedo Speed Up", Tag.PowerUp, owner, 2f)
    {
        protected override void Use()
        {
            owner.EffectTarget.AddEffect(new TorpedoSpeedUpEffect(owner));
        }
    }
}
