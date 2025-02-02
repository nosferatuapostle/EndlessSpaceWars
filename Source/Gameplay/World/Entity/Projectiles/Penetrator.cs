using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class Penetrator : Projectile
    {
        public Penetrator(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\Penetrator" }, position, new Vector2(9, 12), owner, target)
        {
            Name = "Penetrator";
            damage = 1f;
            speed = 200f;

            Cooldown = new CountdownTimer(1f);
            Range = 300f;

            AddAnimation(0, "base", 8);
            SetAnimation("base");
        }
    }
}
