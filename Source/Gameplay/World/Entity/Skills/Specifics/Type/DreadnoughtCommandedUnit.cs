using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class DreadnoughtCommandedUnit : Skill
    {
        protected int total_count;
        protected Unit unit;

        List<Unit> unit_list;

        public DreadnoughtCommandedUnit(Unit summoner, List<Unit> unit_list) : base("Dreadnought Commanded Unit", Tag.Attack, summoner, 25f)
        {
            this.unit_list = unit_list;
        }

        public override void Update(GameTime game_time)
        {
            base.Update(game_time);
            if (owner.IsDead)
            {
                unit.Kill();
                return;
            }
        }

        protected override void Use()
        {
            if (unit != null && unit.IsDead) total_count--;
            if (total_count >= 1)
            {
                unit.Kill();
                total_count--;
            }

            unit = GameGlobals.CommandedNPC(owner, unit_list);
            EntityManager.PassUnit(unit);
            total_count++;
        }
    }
}
