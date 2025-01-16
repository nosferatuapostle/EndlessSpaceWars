using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using System.Collections.Generic;
using System.Linq;

namespace EndlessSpace
{
    public class EncounterZone
    {
        PlayerCharacter player;
        List<Unit> unit_list;
        EntityFactory factory;

        CountdownTimer timer;

        public EncounterZone(PlayerCharacter player, List<Unit> unit_list)
        {
            this.player = player;
            this.unit_list = unit_list;
            factory = new EntityFactory(unit_list);

            timer = new CountdownTimer(2f);

            //SpawnGroup();
            FactionClash();
        }

        public void Update(GameTime game_time)
        {
            timer.Update(game_time);
            if (timer.State == TimerState.Completed && EntityManager.UnitsCount <= 120)
            {
                //SpawnNPCGroup();
                timer.Restart();
            }
        }

        private void FactionClash()
        {
            Vector2 offset = new Vector2(NPC.RADIUS - 5f, 0);

            UnitFaction faction_1 = RandomFaction();
            UnitFaction faction_2 = RandomFaction();

            do
            {
                faction_1 = RandomFaction();
                faction_2 = RandomFaction();
            }
            while(faction_1 == faction_2);

            Vector2 position = Position();

            SpawnGroup(faction_1, position);
            SpawnGroup(faction_2, position + offset);
        }

        private void SpawnGroup(UnitFaction faction, Vector2 position)
        {
            NPC npc_owner = factory.RandomUnit(faction, position, null, null, true, new int[] { 0, 2 });

            Vector2[] positions = ObjectPosition(position, 3);

            NPC npc_1 = factory.RandomUnit(faction, positions[0], npc_owner, null, false, new int[] { 0, 2 });
            NPC npc_2 = factory.RandomUnit(faction, positions[0], npc_owner, null, false, new int[] { 0, 2 });

            npc_owner.group.Add(npc_1);
            npc_owner.group.Add(npc_2);

            npc_owner.SetBehavior(Behavior.Initial);
            npc_1.SetBehavior(Behavior.Group);
            npc_2.SetBehavior(Behavior.Group);

            EntityManager.PassUnit(npc_owner);
            EntityManager.PassUnit(npc_1);
            EntityManager.PassUnit(npc_2);
        }

        private UnitFaction RandomFaction()
        {
            var factions = new[] { UnitFaction.Biomantes, UnitFaction.DuskFleet, UnitFaction.IronCorpse };
            return factions[Globals.Random.Next(factions.Length)];
        }

        private Vector2 Position()
        {
            var camera_bounds = World.Camera.BoundingRectangle;
            float x = 0, y = 0;

            int min = (int)NPC.RADIUS/2;
            int max = (int)NPC.RADIUS*2;

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
            if (position == Vector2.Zero) return Position();
            return new Vector2(x, y);
        }

        private Vector2[] ObjectPosition(Vector2 center, int count, float min_distance = 50f)
        {
            List<Vector2> positions = new() { center };

            for (int i = 1; i < count; i++)
            {
                Vector2 new_position;
                bool valid;

                do
                {
                    float offset_x = Globals.Random.Next(-100, 101);
                    float offset_y = Globals.Random.Next(-100, 101);
                    new_position = center + new Vector2(offset_x, offset_y);

                    valid = positions.All(existing => Vector2.Distance(existing, new_position) >= min_distance);
                }
                while (!valid);

                positions.Add(new_position);
            }

            return positions.ToArray();
        }
    }
}
