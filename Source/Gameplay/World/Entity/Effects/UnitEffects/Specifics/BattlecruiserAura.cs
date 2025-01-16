using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class BattlecruiserAura : UnitEffect
    {
        const string NAME = "Battlecruiser Damage Aura";
        const float RADIUS = 400f;
        
        List<Unit> unit_list, affected_units;
        
        public BattlecruiserAura(Unit source, List<Unit> unit_list) : base("Battlecruiser Aura", source, null, 0.25f, 0f, 0f)
        {
            this.unit_list = unit_list;
            affected_units = new List<Unit>();
        }

        public override void Update(GameTime game_time)
        {
            foreach (var unit in unit_list)
            {
                if (unit == null || unit == source || unit.IsDead || !unit.HostileTo(source)) continue;

                float distance = Vector2.Distance(source.Position, unit.Position);
                if (distance <= RADIUS && !unit.EffectTarget.HasEffect(NAME))
                {
                    UnitEffect effect = new DamageAura(NAME, RADIUS, source, unit, magnitude);
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
