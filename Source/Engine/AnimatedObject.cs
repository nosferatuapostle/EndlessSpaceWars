using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class AnimatedObject : BasicObject
    {
        string[] path;

        int x, y;
        protected Texture2DAtlas[] atlas;
        protected SpriteSheet[] sheets;
        protected AnimatedSprite animated_sprite;
        protected Dictionary<string, AnimatedSprite> animations = new Dictionary<string, AnimatedSprite>();

        public AnimatedObject(string[] path, Vector2 position, Vector2 size) : base(null, position, size)
        {
            this.path = path;
            x = (int)size.X;
            y = (int)size.Y;

            if (path != null && size != Vector2.Zero)
            {
                atlas = new Texture2DAtlas[path.Length];
                sheets = new SpriteSheet[atlas.Length];
                for (int i = 0; i < path.Length; i++)
                {
                    atlas[i] = Texture2DAtlas.Create(null, Globals.Content.Load<Texture2D>(path[i]), x, y);
                    sheets[i] = new SpriteSheet(null, atlas[i]);
                }
            }
        }

        public string[] GetPath => path;
        public Dictionary<string, AnimatedSprite> GetAnimations => animations;

        public int FrameCount => animated_sprite == null ? 0 : animated_sprite.Controller.FrameCount;

        public void AddAnimation(SpriteSheet sprite_sheet, string name, int frame_count, bool is_loop = true, float duration = 0.1f)
        {
            int[] frames = new int[frame_count];
            for (int i = 0; i < frame_count; i++)
            {
                frames[i] = i;
            }

            AddAnimation(sprite_sheet, name, frames, is_loop, duration);
        }

        public void AddAnimation(SpriteSheet sprite_sheet, string name, int[] frames, bool is_loop = true, float duration = 0.1f)
        {
            sprite_sheet.DefineAnimation(name, builder =>
            {
                builder.IsLooping(is_loop);
                for (int i = 0; i < frames.Length; i++)
                {
                    builder.AddFrame(frames[i], TimeSpan.FromSeconds(duration));
                }
            });

            var animated_sprite = new AnimatedSprite(sprite_sheet, name);
            animations[name] = animated_sprite;
        }

        public void SetAnimation(string name)
        {
            if (animations.ContainsKey(name))
            {
                animated_sprite = animations[name];
            }
        }

        public void AddAnimation(int index, string name, int frame_count, bool is_loop = true, float duration = 0.1f)
        {
            if (index >= 0 && index < sheets.Length)
            {
                AddAnimation(sheets[index], name, frame_count, is_loop, duration);
            }
        }

        public void CopyAnimations(Dictionary<string, AnimatedSprite> animations)
        {
            this.animations = animations;
        }

        public virtual void Update(GameTime game_time)
        {
            Bounds.Position = Position;
            animated_sprite?.Update(game_time);
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            if (animated_sprite != null)
            {
                sprite_batch.Draw(animated_sprite, Transform);
            }
        }
    }
}
