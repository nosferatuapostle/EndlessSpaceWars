using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;

namespace EndlessSpace
{
    public class CameraController
    {
        readonly MouseListener mouse_listener;

        public static bool loсk_camera;
        Vector2 new_mouse_pos, prev_mouse_pos;

        public CameraController()
        {
            loсk_camera = false;

            World.Camera.MinimumZoom = 1f;
            World.Camera.MaximumZoom = 2f;

            mouse_listener = new MouseListener();
            mouse_listener.MouseWheelMoved += MouseWheelMovedEvent;
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

        public void Update(GameTime game_time)
        {
            if (loсk_camera || MainMenu.active || SettingsMenu.active) return;
            mouse_listener.Update(game_time);

            new_mouse_pos = Input.MousePosition;

            if (Input.IsMouseButtonDown(MouseButton.Left))
            {
                if (prev_mouse_pos != Vector2.Zero)
                {
                    Vector2 delta = new_mouse_pos - prev_mouse_pos;
                    World.Camera.Position -= delta;
                }
            }

            prev_mouse_pos = new_mouse_pos;
        }
    }
}
