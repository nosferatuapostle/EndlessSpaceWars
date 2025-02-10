using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class UnitEffect
    {
        static ulong next_id = 0;
        public ulong ID { get; private set; } = 0;

        protected PlayerCharacter player;

        protected Unit source, target;
        protected float base_magnitude, magnitude;

        CountdownTimer effect_time;
        CountdownTimer tick_time;

        protected ParticleEffect particle_effect;
        KeywordObject keyword_object;

        protected UnitEffect(string name, Unit source, Unit target, float magnitude, float duration = 0f, float tick_interval = 0.2f)
        {
            ID = ++next_id;

            Name = name;
            keyword_object = new KeywordObject();

            this.source = source;
            this.target = target;
            base_magnitude = magnitude;
            UpdateMagnitude();

            effect_time = duration > 0 ? new CountdownTimer(duration) : null;
            tick_time = tick_interval > 0 ? new CountdownTimer(tick_interval) : null;

            particle_effect = new ParticleEffect();

            source.Event.on_death += OnDeath;
            if (source is PlayerCharacter player) this.player = player;
            if (this.player == null) return;
            this.player.on_level_up += OnLevelUp;
        }

        public static ulong ResetID() => next_id = 0;

        public string Name { get; private set; }
        public bool IsDone { get; protected set; } = false;

        public void AddKeyword(string keyword) => keyword_object.AddKeyword(keyword);
        public bool HasKeyword(string keyword) => keyword_object.HasKeyword(keyword);

        protected void UpdateMagnitude() => magnitude = base_magnitude * source.GetUnitValue(UnitValue.Magnitude);

        void OnDeath(Unit source, Unit target) => IsDone = true;
        protected virtual void OnLevelUp() { }

        public virtual void Update(GameTime game_time)
        {
            particle_effect.Update(game_time.GetElapsedSeconds());

            if (effect_time == null) return;
            effect_time.Update(game_time);
            if (effect_time.State == TimerState.Completed) IsDone = true;

            if (tick_time == null) return;
            tick_time.Update(game_time);
            if (tick_time.State == TimerState.Completed)
            {
                ApplyEffect();
                tick_time.Restart();
            }
        }

        protected virtual void ApplyEffect() { }

        public virtual void OnEffectEnd()
        {
            source.Event.on_death -= OnDeath;
            if (player == null) return;
            player.on_level_up -= OnLevelUp;
        }

        public virtual void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(particle_effect);
        }
    }
}
