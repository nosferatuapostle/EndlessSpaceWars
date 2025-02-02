using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class Rocket : Projectile
    {
        public Rocket(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\Rocket" }, position, new Vector2(9, 16), owner, target)
        {
            Name = "Rocket";
            damage = 1f;
            speed = 200f;

            Cooldown = new CountdownTimer(1f);
            Range = 300f;

            AddAnimation(0, "base", 4);
            SetAnimation("base");
        }
    }
}
