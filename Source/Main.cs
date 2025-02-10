using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace EndlessSpace
{
    public class Main : Game
    {
        public readonly GraphicsDeviceManager graphics;
        SpriteBatch sprite_batch;

        FramesPerSecondCounter frame_counter;

        GameManager game_manager;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Title = "Endless Space";
            Window.AllowUserResizing = true;
            Window.Position = Point.Zero;
            IsMouseVisible = false;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1);
            frame_counter = new FramesPerSecondCounter();

            graphics.HardwareModeSwitch = false;

            graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.PreferredBackBufferHeight = 1080;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            sprite_batch = new SpriteBatch(GraphicsDevice);
            Globals.Save = new Save(1, "EndlessSpace");

            Globals.Content = Content;
            ParticleData.Load(GraphicsDevice);

            Globals.Load();

            game_manager = new GameManager(this, GraphicsDevice, graphics);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            ParticleData.Unload();
        }

        protected override void Update(GameTime game_time)
        {
            frame_counter.Update(game_time);
            Window.Title = $"Endless Space - FPS: {frame_counter.FramesPerSecond}, UNITS: {EntityManager.UnitsCount}";
            Input.Update();

            game_manager.Update(game_time);

            base.Update(game_time);
        }

        protected override void Draw(GameTime game_time)
        {
            GraphicsDevice.Clear(Color.Black); // Black CornflowerBlue
            frame_counter.Draw(game_time);

            game_manager.Draw(sprite_batch, game_time);
            base.Draw(game_time);
        }
    }
}
