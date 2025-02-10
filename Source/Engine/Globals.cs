using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;

namespace EndlessSpace
{
    public delegate void PassObject(object info);
    public delegate object PassObjectAndReturn(object info);

    struct Shader
    {
        public static Effect Throb { get; set; }
        public static Effect GrayScale { get; set; }
        public static Effect Outline { get; set; }
    }

    struct Particle
    {
        public static Texture2D Point { get; set; }
        public static Texture2D Glow { get; set; }
        public static Texture2D HalfCircle { get; set; }
        public static Texture2D Line { get; set; }
        public static Texture2D Line2 { get; set; }
        public static Texture2D Line3 { get; set; }
        public static Texture2D Spark { get; set; }
    }

    struct Font
    {
        public static BitmapFont CourierNew16 { get; set; }
        public static BitmapFont CourierNew16Bold { get; set; }
        public static BitmapFont CourierNew18 { get; set; }
        public static BitmapFont CourierNew18Bold { get; set; }
        public static BitmapFont CourierNew24 { get; set; }
        public static BitmapFont CourierNew24Bold { get; set; }
    }

    public static class ParticleData
    {
        public static void Load(GraphicsDevice graphics_device)
        {
            if (Particle.Point == null)
            {
                Particle.Point = new Texture2D(graphics_device, 1, 1);
                Particle.Point.SetData(new[] { Color.White });
            }
            if (Particle.Line == null)
            {
                Particle.Line = new Texture2D(graphics_device, 4, 1);
                Color[] line_data = new Color[4];
                for (int i = 0; i < line_data.Length; i++)
                {
                    line_data[i] = Color.White;
                }
                Particle.Line.SetData(line_data);
            }

            Particle.Glow = Globals.Content.Load<Texture2D>("Textures\\Particle\\Glow");
            Particle.HalfCircle = Globals.Content.Load<Texture2D>("Textures\\Particle\\HalfCircle");
            Particle.Line2 = Globals.Content.Load<Texture2D>("Textures\\Particle\\Line");
            Particle.Line3 = Globals.Content.Load<Texture2D>("Textures\\Particle\\LineSegment");
            Particle.Spark = Globals.Content.Load<Texture2D>("Textures\\Particle\\Spark");
        }

        public static void Unload()
        {
            Particle.Point?.Dispose();
            Particle.Line?.Dispose();
            Particle.Point = null;
            Particle.Line = null;
        }
    }

    public static class Globals
    {
        public static Random Random { get { return Random.Shared; } }
        public static ContentManager Content { get; set; }

        public static Save Save { get; set; }
        public static string AppData { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); } }

        public static void Load()
        {
            Shader.Throb = Content.Load<Effect>("Shaders\\Throb");
            Shader.GrayScale = Content.Load<Effect>("Shaders\\Grayscale");
            Shader.Outline = Content.Load<Effect>("Shaders\\Outline");

            Font.CourierNew16 = Content.Load<BitmapFont>("Fonts/CourierNew16");
            Font.CourierNew16Bold = Content.Load<BitmapFont>("Fonts/CourierNew16Bold");
            Font.CourierNew18 = Content.Load<BitmapFont>("Fonts/CourierNew18");
            Font.CourierNew18Bold = Content.Load<BitmapFont>("Fonts/CourierNew18Bold");
            Font.CourierNew24 = Content.Load<BitmapFont>("Fonts/CourierNew24");
            Font.CourierNew24Bold = Content.Load<BitmapFont>("Fonts/CourierNew24Bold");
        }
    }
}
