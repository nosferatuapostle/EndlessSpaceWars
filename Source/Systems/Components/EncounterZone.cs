using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EndlessSpace
{
    public class EncounterZone
    {
        PlayerCharacter player;
        List<Unit> unit_list;
        CountdownTimer timer;
        Vector2 offset = new Vector2(NPC.RADIUS - 20f, 0);
        readonly UnitFaction[] factions = new[] { UnitFaction.Biomantes, UnitFaction.DuskFleet, UnitFaction.IronCorpse };

        public EncounterZone(PlayerCharacter player, List<Unit> unit_list)
        {
            this.player = player;
            this.unit_list = unit_list;
            
            timer = new CountdownTimer(5f);
        }

        private UnitFaction RandomFaction() => factions[Globals.Random.Next(factions.Length)];

        public NPC RandomUnit(UnitFaction faction, Vector2 position, int level, Unit owner = null, Unit target = null, bool index_only = false, params int[] index_arr)
        {
            var units = new NPC[]
            {
                new NPC(new Battlecruiser(position, faction), unit_list, level, owner),
                new NPC(new Bomber(position, faction), unit_list, level, owner),
                new NPC(new Dreadnought(position, faction), unit_list, level, owner),
                new NPC(new Fighter(position, faction), unit_list, level, owner),
                new NPC(new Frigate(position, faction), unit_list, level, owner),
                new NPC(new Scout(position, faction), unit_list, level, owner),
                new NPC(new Support(position, faction), unit_list, level, owner),
                new NPC(new Torpedo(position, faction), unit_list, level, owner),
            };

            var filtered_units = index_only
                ? units.Where((unit, idx) => index_arr.Contains(idx)).ToArray()
                : units.Where((unit, idx) => !index_arr.Contains(idx)).ToArray();

            return filtered_units.Length == 0 ? null : filtered_units[Globals.Random.Next(filtered_units.Length)];
        }

        public void Update(GameTime game_time)
        {
            timer.Update(game_time);
            if (timer.State == TimerState.Completed && EntityManager.UnitsCount <= 120)
            {
                HandleEncounter();
                timer.Restart();
            }
        }

        private void HandleEncounter()
        {
            switch (Globals.Random.Next(0, 4))
            {
                case 0: SoloUnit(); break;
                case 1: OneOnOne(); break;
                case 2: SpawnGroup(RandomFaction(), Position()); break;
                case 3: FactionClash(); break;
            }
        }

        private int CalcLevel()
        {
            int level = player.Level;
            int min = (int)MathF.Max(1, level - 12);
            int max = level + 26;
            return Globals.Random.Next(min, max);
        }

        private void SoloUnit()
        {
            NPC npc = RandomUnit(RandomFaction(), Position(), CalcLevel());
            EntityManager.PassUnit(npc);
        }

        private void OneOnOne()
        {
            UnitFaction faction_1, faction_2;
            do
            {
                faction_1 = RandomFaction();
                faction_2 = RandomFaction();
            } while (faction_1 == faction_2);

            Vector2 position = Position();
            NPC npc_1 = RandomUnit(faction_1, position, CalcLevel());
            NPC npc_2 = RandomUnit(faction_2, position + offset, CalcLevel());

            EntityManager.PassUnit(npc_1);
            EntityManager.PassUnit(npc_2);
        }

        private void FactionClash()
        {
            UnitFaction faction_1, faction_2;
            do
            {
                faction_1 = RandomFaction();
                faction_2 = RandomFaction();
            } while (faction_1 == faction_2);

            Vector2 position = Position();
            SpawnGroup(faction_1, position);
            SpawnGroup(faction_2, position + offset);
        }

        private void SpawnGroup(UnitFaction faction, Vector2 position)
        {
            NPC npc_owner = RandomUnit(faction, position, CalcLevel(), null, null, true, new int[] { 0, 2 });
            Vector2[] positions = ObjectPosition(position, 3);

            NPC npc_1 = RandomUnit(faction, positions[0], CalcLevel(), npc_owner, null, false, new int[] { 0, 2 });
            NPC npc_2 = RandomUnit(faction, positions[0], CalcLevel(), npc_owner, null, false, new int[] { 0, 2 });

            npc_owner.group.Add(npc_1);
            npc_owner.group.Add(npc_2);

            npc_owner.SetBehavior(Behavior.Initial);
            npc_1.SetBehavior(Behavior.Group);
            npc_2.SetBehavior(Behavior.Group);

            EntityManager.PassUnit(npc_owner);
            EntityManager.PassUnit(npc_1);
            EntityManager.PassUnit(npc_2);
        }

        private Vector2 Position()
        {
            var camera_bounds = World.Camera.BoundingRectangle;
            int min = (int)(offset.X * 0.8f);
            int max = (int)(offset.X * 1.2f);

            float x = 0, y = 0;

            switch (Globals.Random.Next(0, 4))
            {
                case 0:
                    x = Globals.Random.Next((int)camera_bounds.Left, (int)camera_bounds.Right);
                    y = camera_bounds.Top - Globals.Random.Next(min, max);
                    break;
                case 1:
                    x = Globals.Random.Next((int)camera_bounds.Left, (int)camera_bounds.Right);
                    y = camera_bounds.Bottom + Globals.Random.Next(min, max);
                    break;
                case 2:
                    x = camera_bounds.Left - Globals.Random.Next(min, max);
                    y = Globals.Random.Next((int)camera_bounds.Top, (int)camera_bounds.Bottom);
                    break;
                case 3:
                    x = camera_bounds.Right + Globals.Random.Next(min, max);
                    y = Globals.Random.Next((int)camera_bounds.Top, (int)camera_bounds.Bottom);
                    break;
            }

            Vector2 position = new Vector2(x, y);
            return position == Vector2.Zero && Vector2.Distance(position, player.Position) < NPC.RADIUS ? Position() : position;
        }

        private Vector2[] ObjectPosition(Vector2 center, int count, float min_distance = 50f)
        {
            List<Vector2> positions = new() { center };

            for (int i = 1; i < count; i++)
            {
                Vector2 new_position;
                do
                {
                    float offset_x = Globals.Random.Next(-100, 101);
                    float offset_y = Globals.Random.Next(-100, 101);
                    new_position = center + new Vector2(offset_x, offset_y);
                }
                while (positions.Any(existing => Vector2.Distance(existing, new_position) < min_distance));

                positions.Add(new_position);
            }

            return positions.ToArray();
        }
    }
}
