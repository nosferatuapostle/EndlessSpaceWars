using System;
using System.Diagnostics;

namespace EndlessSpace
{
    public class ScoutEvasion : UnitEffect
    {
        float evade_chance, previous_level, total_magnitude;

        public ScoutEvasion(Unit source) : base("Scout Evasion", source, null, 0.005f, 0)
        {
            evade_chance = base_magnitude * source.Level;
            total_magnitude += evade_chance;
            previous_level = source.Level;

            source.Event.on_attacked += OnAttacked;
        }

        private void OnAttacked(Unit victim, Unit aggressor, ref float damage)
        {
            if (Random.Shared.NextDouble() < MathF.Min(0.5f, evade_chance))
            {
                damage = 0f;
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
                    evade_chance += magnitude_calc;
                    total_magnitude += magnitude_calc;
                }
                else
                {
                    evade_chance -= magnitude_calc;
                    total_magnitude -= magnitude_calc;
                }

                previous_level = player.Level;
            }
        }
    }
}
