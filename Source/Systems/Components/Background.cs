using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace EndlessSpace
{
    public class Background
    {
        GraphicsDevice graphics_device;
        Texture2D starfield;
        Texture2D planet_b;

        public Background(GraphicsDevice graphics_device)
        {
            this.graphics_device = graphics_device;
            starfield = Globals.Content.Load<Texture2D>("Textures\\Background\\starfield_01");
            planet_b = Globals.Content.Load<Texture2D>("Textures\\Background\\planet_01");
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            const float parallax_factor = 0.5f;

            Vector2 camera_position = World.Camera.Position;
            Vector2 starfield_position = camera_position * parallax_factor;
            Vector2 planet_b_position = camera_position * parallax_factor;

            Rectangle destination = World.Camera.BoundingRectangle.ToRectangle();
            PresentationParameters parameters = graphics_device.PresentationParameters;
            destination.Width = parameters.BackBufferWidth * 2;
            destination.Height = parameters.BackBufferHeight * 2;
            destination.X -= parameters.BackBufferWidth * 1;
            destination.Y -= parameters.BackBufferHeight * 1;

            Rectangle starfield_source = new Rectangle(
                (int)Math.Floor(starfield_position.X - destination.Width / 2),
                (int)Math.Floor(starfield_position.Y - destination.Height / 2),
                destination.Width * 2,
                destination.Height * 2
            );


            Vector2 planet_b_screen_position = new Vector2(
                parameters.BackBufferWidth / 4f + planet_b_position.X,
                parameters.BackBufferHeight / 4f + planet_b_position.Y
            );

            sprite_batch.Begin(samplerState: SamplerState.LinearWrap, transformMatrix: World.Camera.GetViewMatrix());
            sprite_batch.Draw(starfield, destination, starfield_source, Color.White);
            sprite_batch.Draw(planet_b, planet_b_screen_position, null, Color.White, 0f, new Vector2(planet_b.Width / 2f, planet_b.Height / 2f), 1f, SpriteEffects.None, 0f);
            sprite_batch.End();
        }



    }
}
