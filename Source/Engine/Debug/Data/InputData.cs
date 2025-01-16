using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = System.Numerics.Vector2;

namespace MonoGame.ImGui.Data
{
    /// <summary>
    ///     Contains the GUIRenderer's input data elements.
    /// </summary>
    public class InputData
    {
        private GraphicsDevice _graphicsDevice;
        private int _scrollwheel;

        public InputData(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _scrollwheel = 0;
        }

        public void Update(Game game)
        {
            if (!game.IsActive)
                return;

            var io = ImGuiNET.ImGui.GetIO();
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();

            UpdateKeyboard(io, keyboard);
            UpdateMouse(io, mouse);

            io.DisplaySize = new Vector2(
                _graphicsDevice.PresentationParameters.BackBufferWidth,
                _graphicsDevice.PresentationParameters.BackBufferHeight
            );
            io.DisplayFramebufferScale = new Vector2(1f, 1f);

            UpdateCursor(io);
        }

        private void UpdateKeyboard(ImGuiIOPtr io, KeyboardState keyboard)
        {
            io.AddKeyEvent(ImGuiKey.Tab, keyboard.IsKeyDown(Keys.Tab));
            io.AddKeyEvent(ImGuiKey.LeftArrow, keyboard.IsKeyDown(Keys.Left));
            io.AddKeyEvent(ImGuiKey.RightArrow, keyboard.IsKeyDown(Keys.Right));
            io.AddKeyEvent(ImGuiKey.UpArrow, keyboard.IsKeyDown(Keys.Up));
            io.AddKeyEvent(ImGuiKey.DownArrow, keyboard.IsKeyDown(Keys.Down));
            io.AddKeyEvent(ImGuiKey.PageUp, keyboard.IsKeyDown(Keys.PageUp));
            io.AddKeyEvent(ImGuiKey.PageDown, keyboard.IsKeyDown(Keys.PageDown));
            io.AddKeyEvent(ImGuiKey.Home, keyboard.IsKeyDown(Keys.Home));
            io.AddKeyEvent(ImGuiKey.End, keyboard.IsKeyDown(Keys.End));
            io.AddKeyEvent(ImGuiKey.Delete, keyboard.IsKeyDown(Keys.Delete));
            io.AddKeyEvent(ImGuiKey.Backspace, keyboard.IsKeyDown(Keys.Back));
            io.AddKeyEvent(ImGuiKey.Enter, keyboard.IsKeyDown(Keys.Enter));
            io.AddKeyEvent(ImGuiKey.Escape, keyboard.IsKeyDown(Keys.Escape));
            io.AddKeyEvent(ImGuiKey.A, keyboard.IsKeyDown(Keys.A));
            io.AddKeyEvent(ImGuiKey.C, keyboard.IsKeyDown(Keys.C));
            io.AddKeyEvent(ImGuiKey.V, keyboard.IsKeyDown(Keys.V));
            io.AddKeyEvent(ImGuiKey.X, keyboard.IsKeyDown(Keys.X));
            io.AddKeyEvent(ImGuiKey.Y, keyboard.IsKeyDown(Keys.Y));
            io.AddKeyEvent(ImGuiKey.Z, keyboard.IsKeyDown(Keys.Z));

            io.KeyShift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            io.KeyCtrl = keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl);
            io.KeyAlt = keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt);
            io.KeySuper = keyboard.IsKeyDown(Keys.LeftWindows) || keyboard.IsKeyDown(Keys.RightWindows);
        }

        private void UpdateMouse(ImGuiIOPtr io, MouseState mouse)
        {
            io.MousePos = new Vector2(mouse.X, mouse.Y);
            io.MouseDown[0] = mouse.LeftButton == ButtonState.Pressed;
            io.MouseDown[1] = mouse.RightButton == ButtonState.Pressed;
            io.MouseDown[2] = mouse.MiddleButton == ButtonState.Pressed;

            var scrollDelta = mouse.ScrollWheelValue - _scrollwheel;
            io.MouseWheel = scrollDelta / 120f;
            _scrollwheel = mouse.ScrollWheelValue;
        }

        private void UpdateCursor(ImGuiIOPtr io)
        {
            var imguiCursor = ImGuiNET.ImGui.GetMouseCursor();
            if (imguiCursor == ImGuiMouseCursor.None)
            {
                // Скрыть курсор
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(-1, -1);
            }
            else
            {
                // Показать курсор
                // Здесь можно настроить отображение, если требуется
            }
        }

        public void Initialize(Game game)
        {
            var io = ImGuiNET.ImGui.GetIO();

            game.Window.TextInput += (sender, args) =>
            {
                if (args.Character != '\t')
                {
                    io.AddInputCharacter(args.Character);
                }
            };

            io.Fonts.AddFontDefault();
        }
    }
}
