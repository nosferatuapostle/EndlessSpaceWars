using System;
using System.Linq;

namespace EndlessSpace
{
    public class IronCorpseOverload(Unit owner) : Skill("Iron Corpse Overload", Tag.PowerUp, owner, 2f)
    {
        protected override void Use()
        {
            owner.EffectTarget.AddEffect(new IronCorpseOverloadEffect(owner));
        }
    }
}
