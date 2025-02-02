using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class IronBomb : Projectile
    {
        public IronBomb(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\IronBomb" }, position, new Vector2(16, 16), owner, target)
        {
            Name = "Iron Bomb";
            damage = 1f;
            speed = 200f;

            Cooldown = new CountdownTimer(1f);
            Range = 300f;

            AddAnimation(0, "base", 16);
            SetAnimation("base");
        }
    }
}
