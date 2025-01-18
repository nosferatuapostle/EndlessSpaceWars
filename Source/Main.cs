using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace EndlessSpace
{
    public class Main : Game
    {
        readonly GraphicsDeviceManager graphics_device;
        SpriteBatch sprite_batch;

        FramesPerSecondCounter frame_counter;

        GameManager game_manager;

        public Main()
        {
            graphics_device = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Endless Space";
            Window.AllowUserResizing = true;
            Window.Position = Point.Zero;
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60f);
            IsFixedTimeStep = true;
            frame_counter = new FramesPerSecondCounter();

            graphics_device.PreferredBackBufferWidth = 980; //1920
            graphics_device.PreferredBackBufferHeight = 620; //1080;
            graphics_device.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprite_batch = new SpriteBatch(GraphicsDevice);

            Globals.Content = Content;
            ParticleData.Load(GraphicsDevice);

            Shader.Throb = Content.Load<Effect>("Shaders\\Throb");
            Shader.Grayscale = Content.Load<Effect>("Shaders\\Grayscale");
            Shader.Outline = Content.Load<Effect>("Shaders\\Outline");
            Shader.OutlineTransparent = Content.Load<Effect>("Shaders\\OutlineTransparent");

            game_manager = new GameManager(this, GraphicsDevice, Window);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            ParticleData.Unload();
        }

        protected override void Update(GameTime game_time)
        {
            if (Input.IsKeyDown(Keys.Escape))
                Exit();

            frame_counter.Update(game_time);
            Window.Title = $"Endless Space - FPS: {frame_counter.FramesPerSecond}, UNITS: {EntityManager.UnitsCount}";

            Input.Update();

            //var elapsedTime = MeasureExecutionTime(() => game_manager.Update(game_time));
            //Debug.WriteLine($"Scope executed in {elapsedTime} ms");
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
