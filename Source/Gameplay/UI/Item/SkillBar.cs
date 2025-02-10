using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class SkillBar
    {
        const float buttonSpacing = 80;

        private PlayerCharacter player;
        private List<Button> skill_buttons;
        private int selected_skill_index = -1;
        private bool awaiting_skill_use = false;

        public SkillBar(PlayerCharacter player, GraphicsDevice graphics_device)
        {
            this.player = player;
            skill_buttons = new List<Button>();

            Vector2 center = new Vector2(graphics_device.Viewport.Width / 2f, graphics_device.Viewport.Height - 50);
            
            string[] skill_keys = { "Q", "W", "E", "R" };

            for (int i = 0; i < 4; i++)
            {
                var button = new Button(null, center + new Vector2((i - 1.5f) * buttonSpacing, 0), new Vector2(60, 60), Color.Gray, Font.CourierNew24, skill_keys[i]);
                button.outline_rect = true;
                int skill_index = i;
                button.on_active += () =>
                {
                    SelectSkill(skill_index);

                };
                skill_buttons.Add(button);
            }
        }

        public void UpdateLayout(GraphicsDevice graphics_device)
        {
            Vector2 center = new Vector2(graphics_device.Viewport.Width / 2f, graphics_device.Viewport.Height - 50);
            foreach (var button in skill_buttons)
            {
                button.Position = center + new Vector2((skill_buttons.IndexOf(button) - 1.5f) * buttonSpacing, 0);
            }
        }

        private void SelectSkill(int index)
        {
            if (index < player.Skills.Count)
            {
                selected_skill_index = index;
                awaiting_skill_use = true;
            }
        }

        public void Update(GameTime game_time)
        {
            if (StatsMenu.active) return;
            skill_buttons.ForEach(b => b.Update(game_time));

            if (awaiting_skill_use && Input.WasLeftClick)
            {
                player.current_skill = player.Skills[selected_skill_index];
                player.used_skills.Add(player.current_skill);
                player.current_skill.Activate();
                awaiting_skill_use = false;
            }
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            if (StatsMenu.active) return;
            skill_buttons.ForEach(b => b.Draw(sprite_batch));
        }
    }
}
