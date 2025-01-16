using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace EndlessSpace
{
    public class AnimatedEngine : AnimatedObject
    {
        Unit unit;
        Vector2 offset;

        public AnimatedEngine(Unit unit, string[] path, Vector2 offset, int frames) : base(path, Vector2.Zero, Vector2.Zero)
        {
            this.unit = unit;
            this.offset = offset;

            Position = unit.Position + offset;
            Rotation = unit.Rotation;

            atlases = new Texture2DAtlas[path.Length];
            sheets = new SpriteSheet[atlases.Length];

            for (int i = 0; i < path.Length; i++)
            {
                atlases[i] = Texture2DAtlas.Create(null, Globals.Content.Load<Texture2D>(path[i]), unit.Width, unit.Height);
                sheets[i] = new SpriteSheet(null, atlases[i]);
            }

            AddAnimation(0, "base", frames);
            SetAnimation("base");
        }

        public void SetUnit(Unit unit) => this.unit = unit;

        public override void Update(GameTime game_time)
        {
            Position = unit.Position + offset;
            Rotation = unit.Rotation;

            base.Update(game_time);
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            if (!unit.IsDead)
            {
                base.Draw(sprite_batch);
            }
        }
    }
}
