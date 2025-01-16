using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EndlessSpace
{
    public class EffectTarget
    {
        Unit unit;

        List<UnitEffect> unit_effects = new List<UnitEffect>();

        VisualFX visual;

        public EffectTarget(Unit unit)
        {
            this.unit = unit;
            visual = new VisualFX();
        }

        public bool IsThrob => visual.is_throb;
        public Color ThrobColor => visual.throb_color;
        public CountdownTimer ThrobTimer => visual.ThrobTimer;

        public List<UnitEffect> ActiveEffects => unit_effects;

        public void AddEffect(UnitEffect unit_effect) => unit_effects.Add(unit_effect);

        public void RemoveEffect(UnitEffect unit_effect)
        {
            unit_effect.OnEffectEnd();
            unit_effects.Remove(unit_effect);
        }

        public void RemoveEffect(string name)
        {
            foreach (UnitEffect effect in unit_effects)
            {
                if (effect.Name == name)
                {
                    effect.OnEffectEnd();
                    unit_effects.Remove(effect);
                    break;
                }
            }
        }

        public bool HasEffect<T>() where T : UnitEffect => unit_effects.Any(e => e is T);
        public bool HasEffect(string name) => unit_effects.Any(e => e.Name == name);
        public bool HasEffect(UnitEffect effect) => unit_effects.Contains(effect);

        public void ActivateThrob(Color color)
        {
            visual.is_throb = true;
            visual.throb_color = color;
        }

        public void Update(GameTime game_time)
        {
            visual.Update(game_time);

            for (int i = unit_effects.Count - 1; i >= 0; i--)
            {
                var effect = unit_effects[i];
                effect.Update(game_time);

                if (effect.IsDone)
                {
                    effect.OnEffectEnd();
                    unit_effects.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            foreach (UnitEffect effect in unit_effects)
            {
                effect.Draw(sprite_batch);
            }
        }
    }
}
