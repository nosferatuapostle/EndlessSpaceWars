using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public PlayerCharacter PlayerCharacter { get; private set; } = null;
        public bool IsBoss { get; private set; } = false;

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
            if (type is Asteroid) return;
            detected_units.Clear();

            foreach (var unit in unit_list)
            {
                if (unit == null || unit == this || unit.IsDead) continue;

                if (detection_radius.Contains(unit.Position))
                {
                    detected_units.Add(unit);
                }
            }
        }

        public override void Attack(Unit target, float delta_time)
        {
            if (target == null || target.IsDead) return;

            Vector2 direction_to_target = target.Position - Position;
            float distance_to_target = direction_to_target.Length();

            if (distance_to_target > 0)
            {
                direction_to_target /= distance_to_target;
                direction_to_target = AI.AdjustDirection(direction_to_target, delta_time) * distance_to_target;
            }

            if (weapon.Range < distance_to_target)
            {
                Vector2 adjusted_position = Position + direction_to_target;
                MoveTo(adjusted_position);
            }
            else
            {
                MoveStop();
                Rotate(target.Position, delta_time);
                weapon.PassProjectile(target, delta_time);
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
            if (PlayerCharacter == null)
            {
                PlayerCharacter = unit_list?.OfType<PlayerCharacter>().FirstOrDefault();
                if (PlayerCharacter == null) return;
                if (Level >= PlayerCharacter.Level + 12) IsBoss = true;
            }

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
                AddKillReward(player, dying);
            }
            base.OnDeath(dying, killer);
        }
    }
}
