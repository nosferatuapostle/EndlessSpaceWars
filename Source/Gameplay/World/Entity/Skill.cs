using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System.Collections.Generic;

namespace EndlessSpace
{
    public abstract class Skill
    {
        public enum Tag
        {
            None,
            Escape,
            Attack,
            PowerUp,
            Heal
        }

        KeywordObject keyword_object;

        protected Unit owner;
        CountdownTimer cooldown;
        float time;

        public Skill(string name, Tag tag, Unit owner, float time)
        {
            Name = name;
            this.owner = owner;
            this.time = time;

            cooldown = new CountdownTimer(time);
            cooldown.Stop();

            keyword_object = new KeywordObject();
            keyword_object.AddKeyword(tag.ToString());
        }

        public string Name { get; private set; }
        public bool IsReady => cooldown.State == TimerState.Completed || cooldown.State == TimerState.Stopped;

        public List<string> Tags => keyword_object.keyword_list;
        public bool HasTag(Tag tag) => keyword_object.HasKeyword(tag.ToString());

        public virtual void Update(GameTime game_time)
        {
            cooldown.Update(game_time);
        }

        public void Activate()
        {
            if (IsReady)
            {
                Use();
                cooldown.Restart();
            }
        }

        protected abstract void Use();
    }
}
