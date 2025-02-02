using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace EndlessSpace
{
    public class BattlecruiserAuraProj : InvisProj
    {
        public BattlecruiserAuraProj(Vector2 position, Unit owner) : base(position, new Vector2(600, 600), owner, null, 5f)
        {
            Name = "Battlecruiser Aura";
            damage = 0.25f;
            speed = 0f;
        }

        public void Done() => is_done = true;

        protected override void HitAction(Unit target)
        {
            if (target.EffectTarget.HasEffect(Name)) return;
            target.EffectTarget.AddEffect(new DamageAura(Name, owner, target, damage, Color.Red));
        }
    }
}
