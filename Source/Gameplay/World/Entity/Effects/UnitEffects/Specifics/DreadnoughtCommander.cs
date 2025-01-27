using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class DreadnoughtCommander : UnitEffect
    {
        int total_count;
        Unit unit;

        List<Unit> unit_list;

        CountdownTimer resurrection_time;

        public DreadnoughtCommander(Unit source, List<Unit> unit_list) : base("Dreadnought Commander", source, null, 0f, 0f, 0f)
        {
            total_count = 0;
            this.unit_list = unit_list;

            resurrection_time = new CountdownTimer(2f);
        }

        protected override void OnLevelChanged()
        {
            ApplyEffect();
        }

        public override void Update(GameTime game_time)
        {
            base.Update(game_time);
            if (unit != null && !unit.IsDead) return;
            resurrection_time.Update(game_time);
            if (resurrection_time.State == TimerState.Completed)
            {
                ApplyEffect();
                resurrection_time.Restart();
            }
        }

        protected override void ApplyEffect()
        {
            if (unit != null && unit.IsDead) total_count--;
            if (total_count >= 1)
            {
                unit.Kill();
                total_count--;
            }

            unit = CommandedNPC();
            EntityManager.PassUnit(unit);
            total_count++;
        }

        private NPC CommandedNPC()
        {
            float distance = source.Size.X;
            float rotation = source.Rotation;

            float x_offset = distance * MathF.Cos(rotation - MathF.PI / 2f);
            float y_offset = distance * MathF.Sin(rotation - MathF.PI / 2f);

            Vector2 position = source.Position + new Vector2(x_offset, y_offset);

            switch (source.Level)
            {
                case 1:
                    return new NPC(new Scout(source.Position + new Vector2(x_offset, y_offset), source.Faction), unit_list, source.Level, source);
                case 2:
                    return new NPC(new Fighter(source.Position + new Vector2(x_offset, y_offset), source.Faction), unit_list, source.Level, source);
                case 3:
                    return new NPC(new Frigate(source.Position + new Vector2(x_offset, y_offset), source.Faction), unit_list, source.Level, source);
                case 4:
                    return new NPC(new Torpedo(source.Position + new Vector2(x_offset, y_offset), source.Faction), unit_list, source.Level, source);
                case 5:
                    return new NPC(new Support(source.Position + new Vector2(x_offset, y_offset), source.Faction), unit_list, source.Level, source);
                default:
                    return new NPC(new Scout(source.Position + new Vector2(x_offset, y_offset), source.Faction), unit_list, source.Level, source);
            }
        }
    }
}
