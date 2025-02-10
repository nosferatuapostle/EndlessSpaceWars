using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace EndlessSpace
{
    public class BasicObject
    {
        Texture2D texture;
        Vector2 pos, dims, scale;
        float rot;
        Color color;

        public Rectangle? source_rectangle = null;

        public BasicObject(string path, Vector2 position, Vector2 size)
        {
            if (path != null)
            {
                texture = Globals.Content.Load<Texture2D>(path);
            }

            pos = position;
            dims = size;
            scale = Vector2.One;
            rot = 0f;
            color = Color.White;

            source_rectangle = null;

            Transform = new Transform2
            {
                Position = position,
                Rotation = rot,
                Scale = scale
            };
            
            float radius = (float)Math.Sqrt(size.X * size.X + size.Y * size.Y);
            Bounds = new CircleF(position, radius/2f);
        }

        public Transform2 Transform { get; private set; }

        public Texture2D Texture => texture;

        public Vector2 Position
        {
            get => Transform.Position;
            set
            {
                Transform.Position = value;
                pos = value;
            }
        }

        public Vector2 Size
        {
            get { return dims * scale; }
            set { dims = value; }
        }

        public Vector2 Scale
        {
            get => Transform.Scale;
            set
            {
                Transform.Scale = value;
                scale = value;
            }
        }

        public float Rotation
        {
            get => Transform.Rotation;
            set
            {
                Transform.Rotation = value;
                rot = value;
            }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public IShapeF Bounds { get; protected set; }
        
        public virtual bool IsHovered()
        {
            return GetRectangle().Contains(Input.MousePosition.ToPoint().ToVector2());
        }

        public virtual bool IsHoveredInWorld()
        {
            return GetRectangle().Contains(World.Camera.ScreenToWorld(Input.MousePosition.ToPoint().ToVector2()));
        }

        public virtual RectangleF GetRectangle()
        {
            return new RectangleF(pos - Size / 3f, Size / 1.5f);
        }

        public virtual void Draw(SpriteBatch sprite_batch)
        {
            if (texture != null)
            {
                Vector2 adjusted_origin = new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2) * Scale;
                sprite_batch.Draw(texture, new Rectangle((int)(pos.X), (int)(pos.Y), (int)Size.X, (int)Size.Y), source_rectangle, color, Transform.Rotation, adjusted_origin, new SpriteEffects(), 0);
            }
        }

        public virtual void Draw(SpriteBatch sprite_batch, Vector2 offset, Vector2 origin, Color color)
        {
            if (Texture != null)
            {
                sprite_batch.Draw(Texture, new Rectangle((int)(pos.X + offset.X), (int)(pos.Y + offset.Y), (int)Size.X, (int)Size.Y), source_rectangle, color, Transform.Rotation, new Vector2(origin.X, origin.Y), new SpriteEffects(), 0);
            }
        }
    }
}
