﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndlessSpace
{
    public class EncounterZone
    {
        const int NPC_COUNT = 100;

        PlayerCharacter player;
        List<Unit> unit_list;
        Vector2 offset;
        readonly UnitFaction[] factions;
        HashSet<ulong> asteroid_ids;

        public EncounterZone(PlayerCharacter player, List<Unit> unit_list)
        {
            this.player = player;
            this.unit_list = unit_list;
            offset = new Vector2(Globals.Random.Next((int)NPC.RADIUS / 2, (int)NPC.RADIUS), 0);
            factions = new[] { UnitFaction.Biomantes, UnitFaction.DuskFleet, UnitFaction.IronCorpse };
            asteroid_ids = new HashSet<ulong>();

            SpaceStation(Position());

            var camera_bounds = World.Camera.BoundingRectangle;
            var spawn_margin = 50f;
            for (int i = 0; i < Globals.Random.Next(11, 26); i++)
            {
                float x = Globals.Random.NextSingle(camera_bounds.Left + spawn_margin, camera_bounds.Right - spawn_margin);
                float y = Globals.Random.NextSingle(camera_bounds.Top + spawn_margin, camera_bounds.Bottom - spawn_margin);

                Vector2 asteroid_position = new Vector2(x, y);

                if (Vector2.Distance(asteroid_position, player.Position) > spawn_margin)
                {
                    NPC asteroid = new NPC(new Asteroid(asteroid_position), unit_list, 1);
                    asteroid.SetBehavior(Behavior.None);
                    EntityManager.PassUnit(asteroid);
                }
            }

            Task.Run(async () => await AsyncSpawn());
        }

        async Task AsyncSpawn()
        {
            while (true)
            {
                while (true/*MainMenu.active*/) await Task.Delay(1200);

                SpawnAsteroid();
                await Task.Delay(Globals.Random.Next(1000, 3000));
                if (EntityManager.UnitsCount <= NPC_COUNT)
                {
                    HandleEncounter();
                }
            }
        }

        private void SpawnAsteroid()
        {
            if (asteroid_ids.Count > NPC_COUNT/2f)
            {
                foreach (var id in asteroid_ids)
                {
                    if (!unit_list.Any(unit => (unit as Character)?.ID == id))
                    {
                        asteroid_ids.Remove(id);
                    }
                }
                return;
            }

            Vector2 asteroid_position = Position();

            NPC asteroid = new NPC(new Asteroid(asteroid_position), unit_list, Math.Max(1, player.Level/2));
            asteroid_ids.Add(asteroid.ID);
            asteroid.SetBehavior(Behavior.None);
            EntityManager.PassUnit(asteroid);
        }

        private UnitFaction RandomFaction() => factions[Globals.Random.Next(factions.Length)];

        public NPC RandomUnit(UnitFaction faction, Vector2 position, int level, Unit owner = null, bool index_only = false, params int[] index_arr)
        {
            var unit_types = new Unit[]
            {
                new Battlecruiser(position, faction),
                new Bomber(position, faction),
                new Dreadnought(position, faction),
                new Fighter(position, faction),
                new Frigate(position, faction),
                new Scout(position, faction),
                new Support(position, faction),
                new Torpedo(position, faction)
            };

            var filtered_units = index_only
                ? unit_types.Where((_, idx) => index_arr.Contains(idx)).ToArray()
                : unit_types.Where((_, idx) => !index_arr.Contains(idx)).ToArray();

            if (filtered_units.Length == 0) return null;

            var selected_unit = filtered_units[Globals.Random.Next(filtered_units.Length)];
            return new NPC(selected_unit, unit_list, level, owner);
        }

        //public void Update(GameTime game_time)
        //{
        //    timer.Update(game_time);
        //    if (timer.State == TimerState.Completed && EntityManager.UnitsCount <= NPC_COUNT)
        //    {
        //        //HandleEncounter();
        //        timer.Restart();
        //    }
        //}

        private void HandleEncounter()
        {
            switch (Globals.Random.Next(0, 5))
            {
                case 0: SoloUnit(); break;
                case 1: OneOnOne(); break;
                case 2: SpawnGroup(RandomFaction(), Position()); break;
                case 3: FactionClash(); break;
                case 4: SpaceStation(Position()); break;
            }
        }

        private int CalcLevel()
        {
            int level = player.Level;
            int min = level - 13;
            int max = level + 25;

            if (min < 1)
            {
                int diff = 1 - min;
                min += diff;
                max -= diff;
            }

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

            Vector2 offset = Globals.Random.NextDouble() < 0.5 ? this.offset : -this.offset;

            SpawnGroup(faction_2, position + offset);
        }

        private void SpawnGroup(UnitFaction faction, Vector2 position)
        {
            NPC npc_owner = RandomUnit(faction, position, CalcLevel(), null, true, new int[] { 0, 2 });
            Vector2[] positions = ObjectPosition(position, 3);
            npc_owner.SetBehavior(Behavior.Group);
            EntityManager.PassUnit(npc_owner);

            for (int i = 0; i < 2; i++)
            {
                NPC npc = RandomUnit(faction, positions[i], CalcLevel(), npc_owner, false, new int[] { 0, 2 });
                npc_owner.group.Add(npc);
                npc.SetBehavior(Behavior.Group);
                EntityManager.PassUnit(npc);
            }
        }

        private void SpaceStation(Vector2 position)
        {
            UnitFaction faction = RandomFaction();

            NPC space_station = new NPC(new SpaceStation(position, faction), unit_list, CalcLevel());
            space_station.SetBehavior(Behavior.Objective);

            int count = Globals.Random.Next(3, 7);

            Vector2[] positions = ObjectPosition(position, count);

            for (int i = 0; i < count; i++)
            {
                NPC npc = RandomUnit(faction, positions[i], CalcLevel(), space_station);
                space_station.group.Add(npc);
                npc.SetBehavior(Behavior.Defense);
                EntityManager.PassUnit(npc);
            }
            EntityManager.PassUnit(space_station);
        }

        private Vector2 Position()
        {
            var camera_bounds = World.Camera.BoundingRectangle;
            int min = (int)(offset.X);
            int max = (int)(offset.X * 2f);

            float x, y;

            switch (Globals.Random.Next(0, 4))
            {
                case 0:
                    x = Globals.Random.Next((int)(camera_bounds.Left - max), (int)(camera_bounds.Right + max));
                    y = camera_bounds.Top - Globals.Random.Next(min, max);
                    break;
                case 1:
                    x = Globals.Random.Next((int)(camera_bounds.Left - max), (int)(camera_bounds.Right + max));
                    y = camera_bounds.Bottom + Globals.Random.Next(min, max);
                    break;
                case 2:
                    x = camera_bounds.Left - Globals.Random.Next(min, max);
                    y = Globals.Random.Next((int)(camera_bounds.Top - max), (int)(camera_bounds.Bottom + max));
                    break;
                case 3:
                    x = camera_bounds.Right + Globals.Random.Next(min, max);
                    y = Globals.Random.Next((int)(camera_bounds.Top - max), (int)(camera_bounds.Bottom + max));
                    break;
                default:
                    x = 0;
                    y = 0;
                    break;
            }

            Vector2 position = new Vector2(x, y);

            return position == Vector2.Zero && Vector2.Distance(position, player.Position) < (NPC.RADIUS * 2f) && unit_list.Any(unit => Vector2.Distance(position, unit.Position) < NPC.RADIUS) ? Position() : position;
        }

        private Vector2[] ObjectPosition(Vector2 center, int count, float min_distance = 50f)
        {
            HashSet<Vector2> positions = new() { center };

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
