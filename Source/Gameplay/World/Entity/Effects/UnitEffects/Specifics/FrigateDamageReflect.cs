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

            source.Event.on_attacked += ReflectDamage;
        }

        private void ReflectDamage(Unit victim, Unit source, ref float damage)
        {
            float resistance = victim.GetUnitValue(UnitValue.DamageResist);
            float magnitude = source.GetUnitValue(UnitValue.Magnitude);
            float reflected_damage = damage * MathF.Min(1f, total_magnitude);
            float finale_damge = reflected_damage.CalcDamage(resistance, magnitude);

            if (finale_damge <= 0f) return;
            if (finale_damge > 0.5f)
                source.UnitInfo.AddFloatingDamage(finale_damge, Color.Red);

            source.RestoreUnitValue(UnitValue.Health, -finale_damge);

            if (source.GetUnitValue(UnitValue.Health) <= 0)
            {
                source.Event.OnDeath(source, victim);
            }
        }

        protected override void OnLevelChanged()
        {
            float level_difference = player.Level - previous_level;

            if (level_difference != 0)
            {
                float magnitude_calc = base_magnitude * Math.Abs(level_difference);

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

        protected override void ApplyEffect()
        {
            total_magnitude += base_magnitude * source.Level;
        }

        public override void OnEffectEnd()
        {
            total_magnitude = 0f;
            base.OnEffectEnd();
        }
    }
}
