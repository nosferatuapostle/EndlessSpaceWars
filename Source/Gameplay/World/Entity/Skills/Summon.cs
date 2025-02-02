using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class Summon : Skill
    {
        protected int total_count;
        protected Unit summon;

        List<Unit> unit_list, summon_list;

        public Summon(Unit summoner, List<Unit> unit_list) : base("Summon", Tag.Attack, summoner, 20f)
        {
            this.unit_list = unit_list;
            summon_list = new List<Unit>();

            owner.Event.on_death += OnDeath;
        }

        void OnDeath(Unit dying, Unit killer)
        {
            foreach (var unit in summon_list)
            {
                unit.Kill();
            }
            summon_list.Clear();
            owner.Event.on_death -= OnDeath;
        }

        protected override void Use()
        {
            total_count = owner.Level / 5 + 1;
            if (summon_list.Count >= total_count)
            {
                summon_list[0].Kill();
                summon_list.RemoveAt(0);
            }
            summon = GameGlobals.SummonedNPC(Name, owner, unit_list);
            EntityManager.PassUnit(summon);
            summon_list.Add(summon);
        }
    }
}
