using Microsoft.Xna.Framework;
using System;

namespace EndlessSpace
{
    public class FighterCorrosion(Unit source, Weapon weapon) : HitEffect("Fighter Corrosion", source, weapon)
    {
        public override void Activate(Unit target)
        {
            target.EffectTarget.AddEffect(new FighterCorrosionDebuff(source, target));
        }
    }
}
