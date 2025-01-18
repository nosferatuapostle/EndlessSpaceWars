using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class NPC : Character
    {
        public const float RADIUS = 480f;

        AI AI;
        List<Unit> unit_list;

        public readonly HashSet<Unit> group;
        public readonly HashSet<Unit> detected_units;

        public NPC(Unit unit, List<Unit> unit_list, int level, Unit owner = null) : base(unit, unit_list)
        {
            AI = new AI(this, owner);
            Level = level;
            UpdateStats(level);
            this.unit_list = unit_list;
            group = new HashSet<Unit>();
            detected_units = new HashSet<Unit>();

            if (owner != null && owner is PlayerCharacter) IsPlayerTeammate = true;
        }

        public Unit owner => AI.owner;
        public Unit current_target => AI.current_target;
        public HashSet<Unit> target_list => AI.target_list;

        public CircleF detection_radius
        {
            get
            {
                return new CircleF(Position, RADIUS);
            }
        }

        public bool IsActive { get; private set; } = true;
        public void ToggleAI() => IsActive = !IsActive;

        public void SetBehavior(Behavior behavior) => AI.SetBehavior(behavior);

        private void Scope(IEnumerable<Unit> unit_list)
        {
            detected_units.Clear();

            foreach (var unit in unit_list)
            {
                if (unit == this || unit.IsDead) continue;

                if (detection_radius.Contains(unit.Position))
                {
                    detected_units.Add(unit);
                }
            }
        }

        public override bool HostileTo(Unit unit)
        {
            if (unit == AI.owner) return false;
            if (AI.target_list.Contains(unit)) return true;
            return base.HostileTo(unit);
        }

        public override void Update(GameTime game_time)
        {
            base.Update(game_time);

            if (!IsDead && IsActive)
            {
                Scope(unit_list);
                AI.Update(game_time.GetElapsedSeconds());
            }
        }

        protected override void OnDeath(Unit dying, Unit killer)
        {
            if (killer is NPC npc && npc.IsPlayerTeammate && npc.AI.owner != null && npc.AI.owner is PlayerCharacter player)
            {
                player.Experience.AddExp(1f);
            }
            base.OnDeath(dying, killer);
        }
    }
}
