using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EndlessSpace
{
    public enum Behavior
    {
        None,

        Default,
        Fearful,
        Adaptive,
        Aggressive,
        Commanded,

        Initial,
        Group
    }

    public class AI
    {
        enum State
        {
            None,

            Idle,
            Moving,
            Following,
            Attacking,
            Distance,
            Escape,
            Defence
        }

        const float MIN_DISTANCE = 20f;
        const float MAX_DISTANCE = 1624f;
        const float COMMANDED_UNIT_DISTANCE = 164f;

        bool commanded_unit_is_ready;
        float speed_mult;

        NPC npc;
        Vector2 direction;
        
        State state;
        Behavior behavior;

        public Unit owner;
        public HashSet<Unit> target_list;
        
        public Unit current_target => target_list
        .Where(unit => unit != owner && unit != npc && !unit.IsDead)
        .OrderBy(unit => unit == owner?.Target)
        .OrderBy(unit => Vector2.Distance(unit.Position, npc.Position))
        .FirstOrDefault();

        public AI(NPC npc, Unit owner)
        {
            this.npc = npc;
            this.owner = owner;
            target_list = new HashSet<Unit>();

            commanded_unit_is_ready = false;

            behavior = Behavior.Default;

            do
            {
                direction = new Vector2(Random.Shared.Next(-1, 2), Random.Shared.Next(-1, 2)) * 12f;
            }
            while (direction == Vector2.Zero);

            npc.Event.on_attacked += OnAttacked;

            if (owner == null) return;
            owner.Event.on_attacked += OnAttacked;
        }

        public void SetBehavior(Behavior behavior) => this.behavior = behavior;

        private void OnAttacked(Unit attacked, Unit aggressor, ref float damage)
        {
            if (aggressor == null || aggressor.IsDead) return;
            
            if (aggressor.Faction == UnitFaction.Summoned && aggressor is NPC npc && npc.owner != null)
            {
                target_list.Add(npc.owner);
            }
            else if (owner == aggressor)
            {
                target_list.Add(attacked);
            }
            else
            {
                target_list.Add(aggressor);
            }
        }

        private void RefreshTarget()
        {
            target_list.RemoveWhere(unit => unit.IsDead || Vector2.Distance(unit.Position, npc.Position) > MAX_DISTANCE);
            foreach (var unit in npc.detected_units)
            {
                bool is_summon_faction = unit.Faction == UnitFaction.Summoned;
                if (owner != null)
                {
                    if (unit.HostileTo(npc) && unit.HostileTo(owner) && !is_summon_faction) target_list.Add(unit);
                } 
                else if (unit.HostileTo(npc) && !is_summon_faction) target_list.Add(unit);
            }
        }

        private void UpdateState()
        {
            State prev_state = state;
            switch (behavior)
            {
                case Behavior.None:
                    return;
                case Behavior.Default:
                    if (owner != null && !owner.IsDead)
                    {
                        behavior = Behavior.Commanded;
                        owner.Event.on_attacked += OnAttacked;
                    }
                    else if (current_target != null && !current_target.IsDead)
                    {
                        state = State.Attacking;
                    }
                    else
                    {
                        state = State.Moving;
                    }
                    break;
                case Behavior.Commanded:
                    if (owner == null || owner.IsDead)
                    {
                        owner.Event.on_attacked -= OnAttacked;
                        behavior = Behavior.Default;
                    }
                    else
                    {
                        CommandedLogic();
                    }
                    break;
                case Behavior.Fearful:
                    if (current_target != null)
                    {
                        state = State.Escape;
                    }
                    else
                    {
                        state = State.Moving;
                    }
                    break;
                case Behavior.Initial:
                    state = State.Idle;
                    break;
                case Behavior.Group:
                    if (owner == null || owner.IsDead)
                    {
                        behavior = Behavior.Default;
                    }
                    else
                    {
                        CommandedLogic();
                    }
                    break;
            }

            HandleState(prev_state, state);
        }

        private void HandleState(State prev_state, State new_state)
        {
            if (prev_state == new_state) return;

            if (prev_state == State.Moving)
            {
                float current_value = npc.GetBaseUnitValue(UnitValue.SpeedMult);
                npc.SetBaseUnitValue(UnitValue.SpeedMult, Math.Max(0, current_value + speed_mult));
                speed_mult = 0;
            }

            if (new_state == State.Moving)
            {
                float current_value = npc.GetBaseUnitValue(UnitValue.SpeedMult);
                speed_mult = current_value * 0.5f;
                npc.SetBaseUnitValue(UnitValue.SpeedMult, Math.Max(0, current_value - speed_mult));
            }
        }

        private void CommandedLogic()
        {
            var target = current_target; //(owner.Target == npc ? null : owner.Target) ??
            if (commanded_unit_is_ready && target != null && !target.IsDead && Vector2.Distance(npc.Position, owner.Position) < COMMANDED_UNIT_DISTANCE + npc.detection_radius.Radius)
            {
                target_list.Add(target);
                state = State.Attacking;
            }
            else
            {
                commanded_unit_is_ready = false;
                target_list.Clear();
                state = State.Following;
            }
        }

        private void ExecuteState(float dtime)
        {
            switch (state)
            {
                case State.None:
                    return;
                case State.Idle:
                    Initial();
                    break;
                case State.Moving:
                    Moving(dtime);
                    break;
                case State.Following:
                    Follow(dtime);
                    break;
                case State.Attacking:
                    npc.Attack(current_target, dtime);
                    break;
                case State.Escape:
                    Escape(dtime);
                    break;
            }
        }

        public void Update(float dtime)
        {
            RefreshTarget();
            UpdateState();
            ExecuteState(dtime);
        }

        private void Initial()
        {
            npc.MoveStop();
            behavior = Behavior.Default;
        }

        private void Moving(float dtime)
        {
            Vector2 adjusted_direction = direction;

            foreach (var other_unit in npc.detected_units)
            {
                if (other_unit == npc || other_unit.IsDead) continue;

                Vector2 to_other_unit = (other_unit.Position + other_unit.Velocity * dtime) - npc.Position;
                float distance = to_other_unit.Length();

                const float NOUN = 0.75f;
                float offset = other_unit.Width * 2f;

                if (distance < offset && Vector2.Dot(Vector2.Normalize(direction), Vector2.Normalize(to_other_unit)) > 0.1f)
                {
                    Vector2 avoidance_vector = new Vector2(-to_other_unit.Y, to_other_unit.X);
                    float avoidance_strength = Math.Max(0, (offset - distance) / offset);

                    if (Vector2.Dot(avoidance_vector, direction) > 0)
                    {
                        adjusted_direction += avoidance_vector * avoidance_strength * NOUN;
                    }
                    else
                    {
                        adjusted_direction -= avoidance_vector * avoidance_strength * NOUN;
                    }
                }
            }

            adjusted_direction = Vector2.Lerp(direction, adjusted_direction, 0.5f);
            Vector2 target_position = npc.Position + adjusted_direction;
            npc.MoveTo(target_position);
        }


        private void Follow(float dtime)
        {
            float offset = owner.Width * 2f;
            if (behavior == Behavior.Commanded)
            {
                CommandedFollow(offset);
            }
            else
            {
                SyncFollow(offset);
            }
        }

        public void SyncFollow(float offset)
        {
            offset /= 2f;
            List<Unit> group_list = new List<Unit>(owner is NPC owner_npc ? owner_npc.group : null);

            int total_units = group_list.Count;

            if (total_units == 0) return;

            int index = group_list.IndexOf(npc);
            if (index == -1) return;

            int units_per_side = (total_units + 1) / 2;

            bool is_left_side = index < units_per_side;

            int side_index = is_left_side ? index : index - units_per_side;

            float side_offset = (side_index + 1) * offset * (is_left_side ? -1 : 1);

            Vector2 position = owner.Position + new Vector2(side_offset, 0);

            if (Vector2.Distance(npc.Position, position) < MIN_DISTANCE)
            {
                commanded_unit_is_ready = true;
                npc.MoveStop();
            }
            else
            {
                npc.MoveTo(position);
            }
        }



        private void CommandedFollow(float offset)
        {
            if (Vector2.Distance(npc.Position, owner.Position) > offset)
            {
                npc.MoveTo(owner.Position);
                return;
            }
            else
            {
                commanded_unit_is_ready = true;
                npc.MoveStop();
            }
        }

        private void Escape(float dtime)
        {
            Vector2 escape_position = npc.Position - current_target.Position;
            npc.MoveTo(npc.Position + escape_position * 12f);
        }
    }
}
