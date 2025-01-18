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

        private void LevelUp()
        {
            player.Level++;
            player.LevelUp();
        }
    }

}
