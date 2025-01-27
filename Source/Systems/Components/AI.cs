using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
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
        Group,
        Defense
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
            Patrol
        }

        const float DIRECTION_MULT = 32f;
        const float MIN_DISTANCE = 20f;
        const float MAX_DISTANCE = 1624f;
        const float COMMANDED_UNIT_DISTANCE = 256f;

        bool commanded_is_ready;

        float speed_mult;
        CountdownTimer timer;

        PlayerCharacter player;

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
            commanded_is_ready = false;

            this.npc = npc;
            this.owner = owner;
            target_list = new HashSet<Unit>();

            behavior = Behavior.Default;

            timer = new CountdownTimer(0f);

            do
            {
                direction = new Vector2(Random.Shared.Next(-1, 2), Random.Shared.Next(-1, 2)) * DIRECTION_MULT;
            }
            while (direction == Vector2.Zero);

            npc.Event.on_attacked += OnAttacked;
            if (owner == null) return;
            owner.Event.on_attacked += OnAttacked;
        }

        public void SetBehavior(Behavior behavior) => this.behavior = behavior;

        private void OnAttacked(Unit attacked, Unit aggressor, ref float damage)
        {
            if (aggressor.Faction == UnitFaction.Summoned && aggressor is NPC npc && npc.owner != null)
            {
                target_list.Add(npc.owner);
            }
            else if (owner != null && !owner.IsDead)
            {
                target_list.Add(aggressor);
                if (owner is NPC owner_npc)
                {
                    owner_npc.target_list.Add(aggressor);
                }
                if (owner == aggressor)
                {
                    target_list.Add(attacked);
                }
            }
            else
            {
                target_list.Add(aggressor);
            }
        }

        private void RefreshTarget()
        {
            target_list.RemoveWhere(unit => unit.IsDead || Vector2.Distance(unit.Position, npc.Position) > MAX_DISTANCE);
            
            if (owner?.Target != null && owner.Target.HostileTo(owner) && Vector2.Distance(owner.Position, owner.Target.Position) < NPC.RADIUS) target_list.Add(owner.Target);

            foreach (var unit in npc.detected_units)
            {
                if ((unit is Character character && character.type is Asteroid) || unit.Faction == UnitFaction.Summoned) continue;
                if (owner != null && (unit.HostileTo(owner) || owner.HostileTo(unit)))
                {
                    target_list.Add(unit);
                    return;
                } 
                else if (owner == null && unit.HostileTo(npc)) target_list.Add(unit);
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
                case Behavior.Group:
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
                case Behavior.Defense:
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
            var target = current_target;
            if (target != null && !target.IsDead && commanded_is_ready && Vector2.Distance(npc.Position, owner.Position) < COMMANDED_UNIT_DISTANCE + NPC.RADIUS)
            {
                target_list.Add(target);
                state = State.Attacking;
            }
            else
            {
                commanded_is_ready = false;
                target_list.Clear();
                if (behavior == Behavior.Defense)
                {
                    state = State.Patrol;
                }
                else
                {
                    state = State.Following;
                }
            }
        }

        private void ExecuteState(float delta_time)
        {
            switch (state)
            {
                case State.None:
                    return;
                case State.Moving:
                    Moving(delta_time);
                    break;
                case State.Following:
                    Follow(delta_time);
                    break;
                case State.Attacking:
                    npc.Attack(current_target, delta_time);
                    break;
                case State.Escape:
                    Escape(delta_time);
                    break;
                case State.Patrol:
                    Patrol(delta_time);
                    break;
            }
        }

        public void Update(float delta_time)
        {
            if (behavior == Behavior.None) return;
            if (player == null && npc.PlayerCharacter != null && !npc.PlayerCharacter.IsDead)
            {
                player = npc.PlayerCharacter;
                Vector2 to_player = player.Position - npc.Position;
                to_player.Normalize();

                direction = Math.Abs(to_player.X) > Math.Abs(to_player.Y) ? new Vector2(Math.Sign(to_player.X), 0) * DIRECTION_MULT : new Vector2(0, Math.Sign(to_player.Y)) * DIRECTION_MULT;
            }
            RefreshTarget();
            UpdateState();
            ExecuteState(delta_time);
        }

        private void Patrol(float delta_time)
        {
            List<Unit> group = owner is NPC owner_npc ? owner_npc.group.ToList() : null;
            if (group == null || group.Count == 0)
                return;

            float group_spacing = owner.Size.X * 2f;
            int total_units = group.Count;
            int index = group.IndexOf(npc);

            if (index == -1)
                return;

            float angle_step = MathHelper.TwoPi / total_units;
            float target_angle = angle_step * index;

            Vector2 patrol_position = owner.Position + new Vector2(
                (float)Math.Cos(target_angle) * group_spacing,
                (float)Math.Sin(target_angle) * group_spacing
            );

            Vector2 to_target = patrol_position - npc.Position;
            Vector2 adjusted_direction = AdjustDirection(to_target, delta_time);
            Vector2 adjusted_position = npc.Position + adjusted_direction * DIRECTION_MULT * delta_time;

            float distance_to_target = Vector2.Distance(npc.Position, adjusted_position);

            if (distance_to_target > MIN_DISTANCE)
            {
                npc.MoveTo(adjusted_position);
            }
            else
            {
                commanded_is_ready = true;
                npc.MoveStop();
            }
        }

        private void Escape(float delta_time)
        {
            Vector2 escape_direction = npc.Position - current_target.Position;
            Vector2 adjusted_escape_direction = AdjustDirection(escape_direction, delta_time);
            Vector2 escape_position = npc.Position + adjusted_escape_direction * DIRECTION_MULT;

            npc.MoveTo(escape_position);
        }

        private void Moving(float delta_time)
        {
            Vector2 adjusted_direction = AdjustDirection(direction, delta_time);
            Vector2 target_position = npc.Position + adjusted_direction;
            npc.MoveTo(target_position);
        }

        private void Follow(float delta_time)
        {
            float offset = owner.Size.X * 2f;
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
            if (owner == null || owner.IsDead) return;
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
                commanded_is_ready = true;
                npc.MoveStop();
            }
            else npc.MoveTo(position);
        }

        private void CommandedFollow(float offset)
        {
            float distance = Vector2.Distance(npc.Position, owner.Position);
            if (distance > offset)
            {
                npc.MoveTo(owner.Position);
                return;
            }
            else
            {
                commanded_is_ready = true;
                npc.MoveStop();
            }
        }

        public Vector2 AdjustDirection(Vector2 original_direction, float delta_time)
        {
            Vector2 adjusted_direction = original_direction;

            foreach (var other_unit in npc.detected_units)
            {
                Vector2 to_other_unit = (other_unit.Position + Vector2.Zero * delta_time) - npc.Position;
                float distance = to_other_unit.Length();

                const float NOUN = 0.75f;
                float offset = other_unit.Size.X * 2f;

                if (distance < offset && Vector2.Dot(original_direction, to_other_unit) > 0.1f)
                {
                    Vector2 avoidance_vector = new Vector2(-to_other_unit.Y, to_other_unit.X);
                    float avoidance_strength = Math.Max(0, (offset - distance) / offset);

                    if (Vector2.Dot(avoidance_vector, original_direction) > 0)
                    {
                        adjusted_direction += avoidance_vector * avoidance_strength * NOUN;
                    }
                    else
                    {
                        adjusted_direction -= avoidance_vector * avoidance_strength * NOUN;
                    }
                }
            }

            return adjusted_direction;
        }
    }
}
