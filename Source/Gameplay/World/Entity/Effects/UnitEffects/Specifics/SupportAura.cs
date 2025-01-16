using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class SupportAura : UnitEffect
    {
        const string NAME = "Support Buff Aura";
        const float RADIUS = 800f;

        List<Unit> unit_list, affected_units;

        UnitValue[] values;

        public SupportAura(Unit source, List<Unit> unit_list) : base("Support Aura", source, null, 1f, 0f, 0f)
        {
            this.unit_list = unit_list;
            affected_units = new List<Unit>();

            values = new UnitValue[]
            {
                UnitValue.Heal,
                UnitValue.DamageResist,
                UnitValue.Magnitude
            };
        }

        public override void Update(GameTime game_time)
        {
            foreach (var unit in unit_list)
            {
                if (unit == null || unit == source || unit.IsDead || unit.HostileTo(source)) continue;

                float distance = Vector2.Distance(source.Position, unit.Position);
                if (distance <= RADIUS && !unit.EffectTarget.HasEffect(NAME))
                {
                    float magnitude_mult = 0.2f;
                    if (unit == source) magnitude_mult = 0f;
                    UnitEffect effect = new BuffAura(NAME, RADIUS, source, unit, values, new float[]{ 0.25f, 0.2f, magnitude_mult }, magnitude);
                    unit.EffectTarget.AddEffect(effect);
                    affected_units.Add(unit);
                }
            }
            base.Update(game_time);
        }

        public override void OnEffectEnd()
        {
            foreach (var unit in affected_units)
            {
                unit.EffectTarget.RemoveEffect(NAME);
            }
            base.OnEffectEnd();
        }
    }
}
