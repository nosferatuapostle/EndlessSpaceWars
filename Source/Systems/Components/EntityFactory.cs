using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EndlessSpace
{
    public class EntityFactory
    {
        List<Unit> unit_list;

        public EntityFactory(List<Unit> unit_list) => this.unit_list = unit_list;

        NPC Battlecruiser(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null) => new NPC(new Battlecruiser(position, faction), unit_list, owner);
        NPC Bomber(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null) => new NPC(new Bomber(position, faction), unit_list, owner);
        NPC Dreadnought(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null) => new NPC(new Dreadnought(position, faction), unit_list, owner);
        NPC Fighter(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null) => new NPC(new Fighter(position, faction), unit_list, owner);
        NPC Frigate(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null) => new NPC(new Frigate(position, faction), unit_list, owner);
        NPC Scout(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null) => new NPC(new Scout(position, faction), unit_list, owner);
        NPC Support(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null) => new NPC(new Support(position, faction), unit_list, owner);
        NPC Torpedo(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null) => new NPC(new Torpedo(position, faction), unit_list, owner);

        public NPC RandomUnit(UnitFaction faction, Vector2 position, Unit owner = null, Unit target = null, bool include_only = false, params int[] index_arr)
        {
            var units = new NPC[]
            {
                Battlecruiser(faction, position, owner, target),
                Bomber(faction, position, owner, target),
                Dreadnought(faction, position, owner, target),
                Fighter(faction, position, owner, target),
                Frigate(faction, position, owner, target),
                Scout(faction, position, owner, target),
                Support(faction, position, owner, target),
                Torpedo(faction, position, owner, target),
            };

            var filtered_units = include_only
                ? units.Where((unit, idx) => index_arr.Contains(idx)).ToArray()
                : units.Where((unit, idx) => !index_arr.Contains(idx)).ToArray();

            if (filtered_units.Length == 0) return null;

            return filtered_units[Globals.Random.Next(filtered_units.Length)];
        }
    }
}
