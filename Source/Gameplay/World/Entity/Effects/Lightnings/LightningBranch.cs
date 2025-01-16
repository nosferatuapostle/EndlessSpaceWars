using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    class LightningBranch : ILightning
    {
        List<LightningBolt> bolts = new List<LightningBolt>();

        public bool IsComplete { get { return bolts.Count == 0; } }
        public Vector2 End { get; private set; }
        private Vector2 direction;

        static Random rand = new Random();

        public LightningBranch(Vector2 start, Vector2 end)
        {
            End = end;
            direction = Vector2.Normalize(end - start);
            Create(start, end);
        }

        public void Update()
        {
            bolts = bolts.Where(x => !x.IsComplete).ToList();
            foreach (var bolt in bolts)
                bolt.Update();
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            foreach (var bolt in bolts)
                bolt.Draw(sprite_batch);
        }

        private void Create(Vector2 start, Vector2 end)
        {
            var main_bolt = new LightningBolt(start, end);
            bolts.Add(main_bolt);

            int num_branches = rand.Next(3, 6);
            Vector2 diff = end - start;

            float[] branch_points = Enumerable.Range(0, num_branches)
                .Select(x => Rand(0, 1f))
                .OrderBy(x => x).ToArray();

            for (int i = 0; i < branch_points.Length; i++)
            {
                Vector2 bolt_start = main_bolt.GetPoint(branch_points[i]);
                Quaternion rot = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(30 * ((i & 1) == 0 ? 1 : -1)));
                Vector2 bolt_end = Vector2.Transform(diff * (1 - branch_points[i]), rot) + bolt_start;
                bolts.Add(new LightningBolt(bolt_start, bolt_end));
            }
        }

        static float Rand(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }
}
