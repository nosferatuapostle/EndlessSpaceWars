using Microsoft.Xna.Framework;
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
                player.current_skill = player.SkillList[0];
                player.current_skill.Active = true;
            }
            if (e.Key == Keys.W)
            {
                player.current_skill = player.SkillList[1];
                player.current_skill.Active = true;
            }
            if (e.Key == Keys.E)
            {
                player.current_skill = player.SkillList[2];
                player.current_skill.Active = true;
            }
        }

        public Unit selected_unit;

        public void Update(GameTime game_time)
        {
            keyboard_listener.Update(game_time);
            float delta_time = game_time.GetElapsedSeconds();

            if (Input.WasKeyPressed(Keys.K)) selected_unit?.Kill();

            player.current_skill?.Update();

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
                //player.Rotation = Rotate(target_pos, delta_time);
            }

            if (target_state == TargetState.Unit && target_unit != null)
            {
                player.Attack(target_unit, delta_time);
                //player.Rotation = Rotate(target_unit.Position, delta_time);
            }
        }

        //private float Rotate(Vector2 position, float delta_time) => player.Rotate(player.Rotation, player.Position.ToAngle(position), delta_time);

        private void InputHandler(List<Unit> unit_list, Vector2 mouse_position)
        {
            if (Input.IsMouseButtonDown(MouseButton.Right))
            {
                target_pos = mouse_position;
                target_state = TargetState.Position;

                foreach (var unit in unit_list)
                {
                    if (unit == null || unit.Faction == UnitFaction.Summoned || unit is PlayerCharacter) continue;
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
