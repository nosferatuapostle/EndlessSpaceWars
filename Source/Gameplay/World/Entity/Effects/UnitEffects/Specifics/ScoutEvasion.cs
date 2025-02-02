using System;

namespace EndlessSpace
{
    public class ScoutEvasion : UnitEffect
    {
        float evade_chance, previous_level;

        public ScoutEvasion(Unit source) : base("Scout Evasion", source, null, 0.005f, 0)
        {
            evade_chance = base_magnitude * source.Level;
            previous_level = source.Level;

            source.Event.on_attacked += OnAttacked;
        }

        private void OnAttacked(Unit victim, Unit aggressor, ref float damage)
        {
            if (Globals.Random.NextDouble() < MathF.Min(0.5f, evade_chance)) damage = 0f;
        }

        protected override void OnLevelUp()
        {
            float level_difference = player.Level - previous_level;

            if (level_difference != 0)
            {
                float magnitude_calc = base_magnitude * MathF.Abs(level_difference);

                if (level_difference > 0)
                {
                    evade_chance += magnitude_calc;
                }
                else
                {
                    evade_chance -= magnitude_calc;
                }

                previous_level = player.Level;
            }
        }

        public override void OnEffectEnd()
        {
            evade_chance = 0f;
            source.Event.on_attacked -= OnAttacked;
            base.OnEffectEnd();
        }
    }
}
