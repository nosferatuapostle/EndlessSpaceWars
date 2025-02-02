using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace EndlessSpace
{
    public class SupportAuraProj : InvisProj
    {
        UnitValue[] values;

        public SupportAuraProj(Vector2 position, Unit owner) : base(position, new Vector2(1200, 1200), owner, null, 5f)
        {
            Name = "Support Aura";
            damage = 0.5f;
            speed = 0f;

            values = new UnitValue[]
            {
                UnitValue.Heal,
                UnitValue.DamageResist,
                UnitValue.Magnitude
            };
        }

        public void Done() => is_done = true;

        protected override bool HitCondition(Unit unit) => !unit.IsDead && !unit.HostileTo(owner);

        protected override void HitAction(Unit target)
        {
            if (target.EffectTarget.HasEffect(Name)) return;
            float magnitude_mult = 0.2f;
            if (target == owner) magnitude_mult = 0f;
            target.EffectTarget.AddEffect(new BuffAura(Name, owner, target, values, new float[] { 0.25f, 0.2f, magnitude_mult }, damage));
        }
    }
}
