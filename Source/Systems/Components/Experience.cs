using System;

namespace EndlessSpace
{
    public class Experience
    {
        float current_xp, next_xp;
        PlayerCharacter player;

        public Experience(PlayerCharacter player)
        {
            current_xp = 0f;
            next_xp = 10f;

            this.player = player;

            //UpdateValues();
        }

        public float CurrentXP => current_xp;
        public float NextXP => next_xp;

        public void AddExp(float amount)
        {
            current_xp += amount;
            while (current_xp >= next_xp)
            {
                current_xp -= next_xp;
                LevelUp();
            }
        }

        public void RemoveExp(float amount)
        {
            current_xp -= amount;
            if (current_xp < 0)
            {
                current_xp = next_xp + (next_xp - (next_xp - current_xp));
                LevelDown();
            }
        }

        private void LevelUp()
        {
            player.Level++;
            //UpdateValues();
            player.OnLevelChanged();
        }

        private void LevelDown()
        {
            player.Level--;
            if (player.Level < 1) player.Level = 1;
            //UpdateValues();
            player.OnLevelChanged();
        }

        private void UpdateValues()
        {
            int level = player.Level;
            player.SetBaseUnitValue(UnitValue.Health, player.GetBaseUnitValue(UnitValue.Health) + level);
            player.SetBaseUnitValue(UnitValue.Magnitude, player.GetBaseUnitValue(UnitValue.Magnitude) + level);
            player.SetBaseUnitValue(UnitValue.HealRate, player.GetBaseUnitValue(UnitValue.HealRate) + level);
            player.SetBaseUnitValue(UnitValue.Heal, player.GetBaseUnitValue(UnitValue.Heal) + level);
            player.SetBaseUnitValue(UnitValue.CriticalChance, player.GetBaseUnitValue(UnitValue.CriticalChance) + level);
            player.SetBaseUnitValue(UnitValue.DamageResist, player.GetBaseUnitValue(UnitValue.DamageResist) + level);
            player.SetBaseUnitValue(UnitValue.SpeedMult, Math.Min((player.GetBaseUnitValue(UnitValue.SpeedMult) + level), 200f));
        }
    }

}
