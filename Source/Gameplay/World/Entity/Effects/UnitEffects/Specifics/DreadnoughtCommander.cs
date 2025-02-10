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
            AddKeyword("specific");

            total_count = 0;
            this.unit_list = unit_list;

            resurrection_time = new CountdownTimer(5f);
        }

        protected override void OnLevelUp()
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

            unit = GameGlobals.CommandedNPC(source, unit_list);
            EntityManager.PassUnit(unit);
            total_count++;
        }
    }
}
