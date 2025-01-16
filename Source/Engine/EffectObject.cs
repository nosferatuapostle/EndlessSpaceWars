using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class EffectObject : AnimatedObject
    {
        public bool is_done, no_timer;

        public CountdownTimer timer;

        public EffectObject(string[] path, Vector2 position, Vector2 size, int sec) : base(path, position, size)
        {
            is_done = false;
            no_timer = false;

            timer = new CountdownTimer(sec);
        }

        public override void Update(GameTime game_time)
        {
            timer.Update(game_time);
            if (timer.State == TimerState.Completed)
            {
                is_done = true;
            }

            base.Update(game_time);
        }
    }
}
