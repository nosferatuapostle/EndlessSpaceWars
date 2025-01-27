using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessSpace
{
    public class HealthBar : ProgressBar
    {
        Unit unit;
        Vector2 unit_size;
        float delayed_value;
        BasicObject delayed_foreground;
        const float ANIMATION_SPEED = 1f;

        public HealthBar(Unit unit) : base(Vector2.Zero, Vector2.Zero)
        {
            this.unit = unit;
            unit_size = unit.Size * unit.Scale;

            size = new Vector2(unit_size.X / 2f, 4f);
            delayed_value = unit.GetUnitValue(UnitValue.Health);
            delayed_foreground = new BasicObject("Textures\\UI\\ProgressBarWhite", Vector2.Zero, size);

            switch (unit.Faction)
            {
                case UnitFaction.Biomantes:
                    color = Color.PaleGoldenrod;
                    delayed_foreground.Color = Color.LightYellow * 0.5f;
                    break;
                case UnitFaction.DuskFleet:
                    color = Color.DarkSlateBlue;
                    delayed_foreground.Color = Color.LightSteelBlue * 0.5f;
                    break;
                case UnitFaction.IronCorpse:
                    color = Color.DarkRed;
                    delayed_foreground.Color = Color.IndianRed * 0.5f;
                    break;
                default:
                    color = Color.LightBlue;
                    delayed_foreground.Color = Color.LightBlue * 0.5f;
                    break;
            }

            if (unit is PlayerCharacter)
            {
                color = Color.Lerp(color, Color.White, 0.3f);
                delayed_foreground.Color = Color.Lerp(delayed_foreground.Color, Color.White, 0.3f);
            }
        }

        public virtual void Update(float delta_time)
        {
            Vector2 target_position = new Vector2((int)(unit.Position.X - size.X / 2f), (int)(unit.Position.Y + unit_size.Y / 2f));

            position = target_position;

            float target_value = unit.GetUnitValue(UnitValue.Health);

            delayed_value = MathHelper.Lerp(delayed_value, target_value, ANIMATION_SPEED * delta_time);
            delayed_foreground.Size = new Vector2(delayed_value / unit.GetBaseUnitValue(UnitValue.Health) * size.X, size.Y);

            base.Update(unit.GetBaseUnitValue(UnitValue.Health), unit.GetUnitValue(UnitValue.Health), delta_time);
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            background.Draw(sprite_batch, position, Vector2.Zero, Color.White);
            delayed_foreground.Draw(sprite_batch, position, Vector2.Zero, delayed_foreground.Color);
            foreground.Draw(sprite_batch, position, Vector2.Zero, color);
        }
    }
}