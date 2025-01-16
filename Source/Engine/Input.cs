using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace EndlessSpace
{
    public class Input
    {
        static KeyboardStateExtended keyboard_state;
        static MouseStateExtended mouse_state;
        static GamePadState gamepad_state;
        
        public static Vector2 MousePosition => mouse_state.Position.ToVector2();
        public static Vector2 MouseWorld => World.Camera.ScreenToWorld(MousePosition);

        public static void Update()
        {
            KeyboardExtended.Update();
            keyboard_state = KeyboardExtended.GetState();

            MouseExtended.Update();
            mouse_state = MouseExtended.GetState();

            gamepad_state = GamePad.GetState(PlayerIndex.One);
        }

        public static bool IsLeftClick => IsMouseButtonDown(MouseButton.Left);
        public static bool IsRightClick => IsMouseButtonDown(MouseButton.Right);
        public static bool WasLeftClick => WasMouseButtonPressed(MouseButton.Left);
        public static bool WasRightClick => WasMouseButtonPressed(MouseButton.Right);

        public static bool IsKeyDown(Keys key)
        {
            return keyboard_state.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return keyboard_state.IsKeyUp(key);
        }

        public static bool WasKeyPressed(Keys key)
        {
            return keyboard_state.WasKeyPressed(key);
        }

        public static bool WasKeyReleased(Keys key)
        {
            return keyboard_state.WasKeyReleased(key);
        }

        public static bool IsMouseButtonDown(MouseButton button)
        {
            return mouse_state.IsButtonDown(button);
        }

        public static bool IsMouseButtonUp(MouseButton button)
        {
            return mouse_state.IsButtonUp(button);
        }
        
        public static bool WasMouseButtonPressed(MouseButton button)
        {
            return mouse_state.WasButtonPressed(button);
        }
        
        public static bool WasMouseButtonReleased(MouseButton button)
        {
            return mouse_state.WasButtonReleased(button);
        }
    }
}
