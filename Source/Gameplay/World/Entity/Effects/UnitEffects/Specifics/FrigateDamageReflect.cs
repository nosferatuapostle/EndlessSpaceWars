using Microsoft.Xna.Framework;
using System;

namespace EndlessSpace
{
    public class FrigateDamageReflect : UnitEffect
    {
        float previous_level, total_magnitude;
        
        public FrigateDamageReflect(Unit source) : base("Frigate Damage Reflect", source, null, 0.01f, 0f, 0f)
        {
            total_magnitude = 0f;
            previous_level = source.Level;

            total_magnitude += base_magnitude * source.Level;

            source.Event.on_attacked += ReflectDamage;
        }

        private void ReflectDamage(Unit victim, Unit source, ref float damage)
        {
            if (source.Faction == UnitFaction.Summoned) return;

            float resistance = victim.GetUnitValue(UnitValue.DamageResist);
            float magnitude = source.GetUnitValue(UnitValue.Magnitude);
            float reflected_damage = damage * MathF.Min(1f, total_magnitude);
            float finale_damage = reflected_damage.Calc(resistance, magnitude);

            if (finale_damage <= 0f) return;
            if (finale_damage > 0.5f)
                source.UnitInfo.AddFloatingDamage(finale_damage, Color.Red);

            source.RestoreUnitValue(UnitValue.Health, -finale_damage);

            if (source.GetUnitValue(UnitValue.Health) <= 0)
            {
                source.Event.OnDeath(source, victim);
            }
        }

        protected override void OnLevelUp()
        {
            float level_difference = player.Level - previous_level;

            if (level_difference != 0)
            {
                float magnitude_calc = base_magnitude * MathF.Abs(level_difference);

                if (level_difference > 0)
                {
                    total_magnitude += magnitude_calc;
                }
                else
                {
                    total_magnitude -= magnitude_calc;
                }

                previous_level = player.Level;
            }
        }

        public override void OnEffectEnd()
        {
            total_magnitude = 0f;
            source.Event.on_attacked -= ReflectDamage;
            base.OnEffectEnd();
        }
    }
}
