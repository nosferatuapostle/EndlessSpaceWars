using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class FlameWave : Projectile
    {
        public FlameWave(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\FlameWave" }, position, new Vector2(64, 64), owner, target)
        {
            Name = "Flame Wave";
            damage = 1f;
            speed = 200f;

            Cooldown = new CountdownTimer(1f);
            Range = 300f;

            AddAnimation(0, "base", 6);
            SetAnimation("base");
        }
    }
}
