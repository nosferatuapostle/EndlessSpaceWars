using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace EndlessSpace
{
    class LightningBolt : ILightning
    {
        const float NORMALIZE = 60f;

        public List<Line> Segments = new List<Line>();
        public Vector2 Start { get { return Segments[0].A; } }
        public Vector2 End { get { return Segments.Last().B; } }
        public bool IsComplete { get { return Alpha <= 0; } }

        public float Alpha { get; set; }
        public float AlphaMultiplier { get; set; }
        public float FadeOutRate { get; set; }
        public Color Tint { get; set; }

        static Random rand = new Random();

        public LightningBolt(Vector2 source, Vector2 dest) : this(source, dest, new Color(0.9f, 0.8f, 1f)) { }

        public LightningBolt(Vector2 source, Vector2 dest, Color color)
        {
            Segments = CreateBolt(source, dest, 2);

            Tint = color;
            Alpha = 1f;
            AlphaMultiplier = 0.6f;
            FadeOutRate = 0.03f;
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            if (Alpha <= 0)
                return;

            foreach (var segment in Segments)
                segment.Draw(sprite_batch, Tint * (Alpha * AlphaMultiplier));
        }

        public virtual void Update(GameTime game_time)
        {
            Alpha -= FadeOutRate * game_time.GetElapsedSeconds() * NORMALIZE;
        }

        protected static List<Line> CreateBolt(Vector2 source, Vector2 dest, float thickness)
        {
            var results = new List<Line>();
            Vector2 tangent = dest - source;
            Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
            float length = tangent.Length();

            List<float> positions = new List<float>();
            positions.Add(0);

            for (int i = 0; i < length / 4; i++)
                positions.Add(Rand(0, 1));

            positions.Sort();

            const float Sway = 80;
            const float Jaggedness = 1 / Sway;

            Vector2 prevPoint = source;
            float prevDisplacement = 0;
            for (int i = 1; i < positions.Count; i++)
            {
                float pos = positions[i];

                float scale = (length * Jaggedness) * (pos - positions[i - 1]);

                float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

                float displacement = Rand(-Sway, Sway);
                displacement -= (displacement - prevDisplacement) * (1 - scale);
                displacement *= envelope;

                Vector2 point = source + pos * tangent + displacement * normal;
                results.Add(new Line(prevPoint, point, thickness));
                prevPoint = point;
                prevDisplacement = displacement;
            }

            results.Add(new Line(prevPoint, dest, thickness));

            return results;
        }

        public Vector2 GetPoint(float position)
        {
            var start = Start;
            float length = Vector2.Distance(start, End);
            Vector2 dir = (End - start) / length;
            position *= length;

            var line = Segments.Find(x => Vector2.Dot(x.B - start, dir) >= position);
            float lineStartPos = Vector2.Dot(line.A - start, dir);
            float lineEndPos = Vector2.Dot(line.B - start, dir);
            float linePos = (position - lineStartPos) / (lineEndPos - lineStartPos);

            return Vector2.Lerp(line.A, line.B, linePos);
        }

        static float Rand(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }

    public class Line
    {
        public Vector2 A;
        public Vector2 B;
        public float Thickness;

        public Line() { }
        public Line(Vector2 a, Vector2 b, float thickness)
        {
            A = a;
            B = b;
            Thickness = thickness;
        }

        public void Draw(SpriteBatch sprite_batch, Color color)
        {
            Vector2 tangent = B - A;
            float rotation = (float)MathF.Atan2(tangent.Y, tangent.X);
            const float ImageThickness = 8;
            float thicknessScale = Thickness / ImageThickness;
            Vector2 capOrigin = new Vector2(Particle.HalfCircle.Width, Particle.HalfCircle.Height / 2f);
            Vector2 middleOrigin = new Vector2(0, Particle.Line.Height / 2f);
            Vector2 middleScale = new Vector2(tangent.Length(), thicknessScale);
            sprite_batch.Draw(Particle.Line, A, null, color, rotation, middleOrigin, middleScale, SpriteEffects.None, 0f);
            sprite_batch.Draw(Particle.HalfCircle, A, null, color, rotation, capOrigin, thicknessScale, SpriteEffects.None, 0f);
            sprite_batch.Draw(Particle.HalfCircle, B, null, color, rotation + MathHelper.Pi, capOrigin, thicknessScale, SpriteEffects.None, 0f);
        }
    }
}
