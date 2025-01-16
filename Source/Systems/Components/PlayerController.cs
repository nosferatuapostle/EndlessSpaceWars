using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System.Diagnostics;

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
        readonly MouseListener mouse_listener;

        PlayerCharacter player;
        List<Unit> unit_list;

        bool loсk_camera = false;
        Vector2 new_mouse_pos, prev_mouse_pos;

        TargetState target_state = TargetState.None;
        Vector2 target_pos = Vector2.Zero;
        Unit target_unit = null;
        bool is_attack = false;

        public PlayerController(PlayerCharacter player, List<Unit> unit_list)
        {
            this.player = player;
            this.unit_list = unit_list;

            World.Camera.MinimumZoom = 1f;
            World.Camera.MaximumZoom = 2f;

            keyboard_listener = new KeyboardListener();
            keyboard_listener.KeyPressed += KeyPressedEvent;

            mouse_listener = new MouseListener();
            mouse_listener.MouseWheelMoved += MouseWheelMovedEvent;
        }

        private void KeyPressedEvent(object s, KeyboardEventArgs e)
        {
            if (e.Key == Keys.F1)
            {
                loсk_camera = !loсk_camera;
            }
            if (e.Key == Keys.Q)
            {
                player.current_skill = player.skill_list[0];
                player.current_skill.Active = true;
            }
            if (e.Key == Keys.W)
            {
                player.current_skill = player.skill_list[1];
                player.current_skill.Active = true;
            }
            if (e.Key == Keys.E)
            {
                player.current_skill = player.skill_list[2];
                player.current_skill.Active = true;
            }
        }

        private void MouseWheelMovedEvent(object s, MouseEventArgs e)
        {
            if (e.ScrollWheelDelta > 0)
            {
                World.Camera.ZoomIn(0.1f);
            }
            if (e.ScrollWheelDelta < 0)
            {
                World.Camera.ZoomOut(0.1f);
            }
        }

        public Unit selected_unit;

        public void Update(GameTime game_time)
        {
            keyboard_listener.Update(game_time);
            mouse_listener.Update(game_time);
            float dtime = game_time.GetElapsedSeconds();

            if (Input.WasKeyPressed(Keys.K)) selected_unit?.Kill();

            player.current_skill?.Update();

            Camera(Input.MousePosition);

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
                player.Rotation = Rotate(target_pos, dtime);
            }

            if (target_state == TargetState.Unit && target_unit != null)
            {
                player.Attack(target_unit, dtime);
                player.Rotation = Rotate(target_unit.Position, dtime);
            }
        }

        private float Rotate(Vector2 position, float dtime) => player.Rotate(player.Rotation, player.Position.ToAngle(position), dtime);

        private void Camera(Vector2 mouse_pos)
        {
            new_mouse_pos = mouse_pos;

            if (Input.IsMouseButtonDown(MouseButton.Left))
            {
                if (prev_mouse_pos != Vector2.Zero)
                {
                    Vector2 delta = new_mouse_pos - prev_mouse_pos;
                    World.Camera.Position -= delta;
                }
            }
            if (loсk_camera)
            {
                World.Camera.LookAt(player.Position);
            }

            prev_mouse_pos = new_mouse_pos;
        }

        private void InputHandler(List<Unit> unit_list, Vector2 mouse_position)
        {
            if (Input.IsMouseButtonDown(MouseButton.Right))
            {
                target_pos = mouse_position;
                target_state = TargetState.Position;

                foreach (var unit in unit_list)
                {
                    if (unit == null || unit is PlayerCharacter) continue;
                    if (unit.IsHovered())
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

                    if (unit.IsHovered())
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
