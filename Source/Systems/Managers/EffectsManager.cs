using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class EffectManager
    {
        public static PassObject PassEffect, PassLightning;

        List<EffectObject> effect_list = new List<EffectObject>();
        List<ILightning> lightning_list = new List<ILightning>();

        public List<ILightning> LightList => lightning_list;

        public void AddEffect(object info)
        {
            effect_list.Add((EffectObject)info);
        }

        public void AddLightning(object info)
        {
            lightning_list.Add((ILightning)info);
        }

        public void Update(GameTime game_time)
        {
            for (int i = effect_list.Count - 1; i >= 0; i--)
            {
                effect_list[i].Update(game_time);

                if (effect_list[i].is_done)
                {
                    effect_list.RemoveAt(i);
                }
            }

            for (int i = lightning_list.Count - 1; i >= 0; i--)
            {
                lightning_list[i].Update();

                if (lightning_list[i].IsComplete)
                {
                    lightning_list.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            foreach (var effect in effect_list)
            {
                effect.Draw(sprite_batch);
            }
            foreach (var light in lightning_list)
            {
                light.Draw(sprite_batch);
            }
        }
    }
}
