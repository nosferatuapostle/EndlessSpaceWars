using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class VisualFX
    {
        public bool is_throb = false;
        public Color throb_color = Color.White;
        CountdownTimer throb_timer;

        public VisualFX() => throb_timer = new CountdownTimer(0.5f);

        public CountdownTimer ThrobTimer => throb_timer;

        public void Update(GameTime game_time)
        {
            if (is_throb)
            {
                throb_timer.Update(game_time);
                if (throb_timer.State == TimerState.Completed)
                {
                    is_throb = false;
                    throb_timer.Restart();
                }
            }
        }
    }
}
