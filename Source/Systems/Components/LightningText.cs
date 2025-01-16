using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    class LightningText
    {
        static Random rand = new Random();

        List<Vector2> text_particles = new List<Vector2>();
        SpriteBatch sprite_batch;
        List<LightningBolt> bolts = new List<LightningBolt>();
        float hue = 4.5f;
        float[] noise;

        public LightningText(GraphicsDevice device, SpriteBatch sprite_batch, SpriteFont font, string text)
        {
            var viewport = device.Viewport;
            this.sprite_batch = sprite_batch;
            text_particles = CreateParticleText(device, font, text);

            Vector2 offset = new Vector2(viewport.Width, viewport.Height) / 2;
            for (int i = 0; i < text_particles.Count; i++)
                text_particles[i] += offset;

            noise = new float[10];
            for (int i = 0; i < 10; i++)
                noise[i] = (float)rand.NextDouble();
        }

        private float GetNoise(float x)
        {
            x = Math.Max(x, 0);
            int length = noise.Length;
            int i = ((int)(length * x)) % length;
            int j = (i + 1) % length;
            return MathHelper.SmoothStep(noise[i], noise[j], x - (int)x);
        }

        public void Update()
        {
            bolts.Clear();

            hue += 0.01f;
            if (hue >= 6)
                hue -= 6;

            foreach (var particle in text_particles)
            {
                float x = particle.X / 500f;

                int boltChance = (int)(20 * Math.Sin(3 * hue * MathHelper.Pi - x + 1 * GetNoise(hue + x)) + 52);

                if (rand.Next(boltChance) == 0)
                {
                    Vector2 nearestParticle = Vector2.Zero;
                    float nearestDist = float.MaxValue;

                    for (int i = 0; i < 50; i++)
                    {
                        var other = text_particles[rand.Next(text_particles.Count)];
                        var dist = Vector2.DistanceSquared(particle, other);

                        if (dist < nearestDist && dist > 10 * 10)
                        {
                            nearestDist = dist;
                            nearestParticle = other;
                        }
                    }

                    if (nearestDist < 200 * 200 && nearestDist > 10 * 10)
                        bolts.Add(new LightningBolt(particle, nearestParticle, Color.AliceBlue));
                }
            }
        }

        public void Draw()
        {
            foreach (var bolt in bolts)
                bolt.Draw(sprite_batch);
        }

        List<Vector2> CreateParticleText(GraphicsDevice device, SpriteFont font, string text)
        {
            Vector2 size = font.MeasureString(text) + new Vector2(0.5f);
            int width = (int)size.X;
            int height = (int)size.Y;

            RenderTarget2D target = new RenderTarget2D(device, width, height);
            device.SetRenderTarget(target);
            device.Clear(Color.Black);

            SpriteBatch sprite_batch = new SpriteBatch(device);
            sprite_batch.Begin();
            sprite_batch.DrawString(font, text, Vector2.Zero, Color.White);
            sprite_batch.End();

            device.SetRenderTarget(null);

            Color[] data = new Color[width * height];
            target.GetData<Color>(data);

            const int interval = 2;
            const float scale = 3.5f;
            List<Vector2> points = new List<Vector2>();
            for (int y = 0; y < height; y += interval)
            {
                for (int x = 0; x < width; x += interval)
                {
                    int tx = Clamp(x + rand.Next(-interval / 2, interval / 2), 0, width - 1);
                    int ty = Clamp(y + rand.Next(-interval / 2, interval / 2), 0, height - 1);
                    if (data[width * ty + tx].R == 255)
                        points.Add((new Vector2(tx, ty) - size / 2) * scale);
                }
            }

            target.Dispose();

            return points;
        }

        int Clamp(int value, int min, int max)
        {
            return value < min ? min : (value > max ? max : value);
        }
    }
}
