using Microsoft.Xna.Framework;
using System;

namespace EndlessSpace
{
    public static class Extensions
    {
        public static Color Invert(this Color color)
        {
            return new Color(255 - color.R, 255 - color.G, 255 - color.B, color.A);
        }

        public static float Calc(this float damage, float resistance, float magnitude)
        {
            float damage_reduction = resistance / (resistance + MathF.PI);
            return damage * magnitude * (1f - damage_reduction);
        }

        public static float ToAngle(this Vector2 source, Vector2 target)
        {
            return MathF.Atan2(target.Y - source.Y, target.X - source.X) + MathHelper.PiOver2;
        }
    }
}
