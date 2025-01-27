using EndlessSpace;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class Background
{
    GraphicsDevice graphics_device;
    Texture2D starfield;
    PlayerCharacter player;
    Vector2 camera_position, starfield_offset;

    public Background(GraphicsDevice graphics_device, PlayerCharacter player)
    {
        this.graphics_device = graphics_device;
        this.player = player;

        starfield = Globals.Content.Load<Texture2D>("Textures\\Background\\starfield");
    }

    public void Update(GameTime game_time)
    {
        camera_position = World.Camera.Position;

        starfield_offset = camera_position * 0.1f;
        starfield_offset.X = WrapCoordinate(starfield_offset.X, starfield.Width);
        starfield_offset.Y = WrapCoordinate(starfield_offset.Y, starfield.Height);
    }

    public void Draw(SpriteBatch sprite_batch)
    {
        sprite_batch.Begin();

        TiledTexture(sprite_batch, starfield, starfield_offset);

        sprite_batch.End();
    }

    private void TiledTexture(SpriteBatch sprite_batch, Texture2D texture, Vector2 offset)
    {
        int screen_width = graphics_device.Viewport.Width;
        int screen_height = graphics_device.Viewport.Height;

        int texture_width = texture.Width;
        int texture_height = texture.Height;

        for (float y = -offset.Y; y < screen_height; y += texture_height)
        {
            for (float x = -offset.X; x < screen_width; x += texture_width)
            {
                sprite_batch.Draw(texture, new Vector2(x, y), Color.White);
            }
        }
    }

    private float WrapCoordinate(float value, float max)
    {
        return ((value % max) + max) % max;
    }
}
