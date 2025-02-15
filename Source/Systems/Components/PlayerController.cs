﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class PlayerController
    {
        enum TargetState
        {
            None,
            Position,
            Unit
        }

        readonly KeyboardListener keyboard_listener;

        PlayerCharacter player;
        List<Unit> unit_list;

        TargetState target_state = TargetState.None;
        Vector2 target_pos = Vector2.Zero;
        Unit target_unit = null;

        public PlayerController(PlayerCharacter player, List<Unit> unit_list)
        {
            this.player = player;
            this.unit_list = unit_list;

            keyboard_listener = new KeyboardListener();
            keyboard_listener.KeyPressed += KeyPressedEvent;
        }

        private void KeyPressedEvent(object s, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Q)
            {
                player.current_skill = player.Skills[0];
                player.used_skills.Add(player.current_skill);
                player.current_skill.Activate();
            }
            if (e.Key == Keys.W)
            {
                player.current_skill = player.Skills[1];
                player.used_skills.Add(player.current_skill);
                player.current_skill.Activate();
            }
            if (e.Key == Keys.E)
            {
                player.current_skill = player.Skills[2];
                player.used_skills.Add(player.current_skill);
                player.current_skill.Activate();
            }
            if (e.Key == Keys.R)
            {
                player.current_skill = player.Skills[3];
                player.used_skills.Add(player.current_skill);
                player.current_skill.Activate();
            }
        }

        public Unit selected_unit;

        public void Update(GameTime game_time)
        {
            keyboard_listener.Update(game_time);
            float delta_time = game_time.GetElapsedSeconds();

            if (Input.WasKeyPressed(Keys.K)) selected_unit?.Kill();

            if (Input.IsKeyDown(Keys.F1))
            {
                CameraController.loсk_camera = true;
                World.Camera.LookAt(player.Position);
            }
            else
            {
                CameraController.loсk_camera = false;
            }

            InputHandler(unit_list, World.Camera.ScreenToWorld(Input.MousePosition));

            var dir_len = (target_pos - player.Position).Length();

            if (target_state == TargetState.Position)
            {
                if (dir_len <= 20f)
                {
                    target_state = TargetState.None;
                }
                else
                {
                    player.MoveTo(target_pos);
                }
            }

            if (target_state == TargetState.Unit && target_unit != null)
            {
                player.Attack(target_unit, delta_time);
            }
        }
        private void InputHandler(List<Unit> unit_list, Vector2 mouse_position)
        {
            if (Input.IsMouseButtonDown(MouseButton.Right))
            {
                target_pos = mouse_position;
                target_state = TargetState.Position;

                foreach (var unit in unit_list)
                {
                    if (unit == null || unit.Faction == UnitFaction.Summoned || unit is PlayerCharacter) continue;
                    if (unit.IsHoveredInWorld())
                    {
                        target_unit = unit;
                        target_state = TargetState.Unit;
                        break;
                    }
                }
            }

            if (Input.WasMouseButtonPressed(MouseButton.Left))
            {
                foreach (var unit in unit_list)
                {
                    if (unit == null) continue;

                    var character = unit as Character;

                    if (unit.IsHoveredInWorld())
                    {
                        if (selected_unit != null) selected_unit.is_selected = false;
                        selected_unit = unit;
                        selected_unit.is_selected = true;
                        break;
                    }
                    else
                    {
                        if (selected_unit != null) selected_unit.is_selected = false;
                        selected_unit = null;
                    }
                }
            }
        }
    }
}
