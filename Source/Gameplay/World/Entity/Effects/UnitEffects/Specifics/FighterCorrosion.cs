using Microsoft.Xna.Framework;
using System;

namespace EndlessSpace
{
    public class FighterCorrosion(Unit source, Weapon weapon) : HitEffect("Fighter Corrosion", source, weapon)
    {
        protected override void Awake()
        {
            AddKeyword("specific");
        }

        public override void Activate(Unit target)
        {
            target.EffectTarget.AddEffect(new FighterCorrosionDebuff(source, target));
        }
    }
}
