using System;

namespace EndlessSpace
{
    public class Experience
    {
        int current_xp, next_xp;
        PlayerCharacter player;

        public Experience(PlayerCharacter player)
        {
            current_xp = 0;
            next_xp = 20;

            this.player = player;
        }

        public int CurrentXP => current_xp;
        public int NextXP => next_xp;

        public void AddExp(int amount)
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
            next_xp += 10;
            player.OnLevelUp();
            player.Level++;
            player.SaveData();
        }
    }

}
