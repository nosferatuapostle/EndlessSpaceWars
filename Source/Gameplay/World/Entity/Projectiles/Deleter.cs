using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class Deleter : Projectile
    {
        public Deleter(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\Deleter" }, position, new Vector2(8, 16), owner, target)
        {
            Name = "Deleter";
            damage = 1f;
            speed = 200f;

            Cooldown = new CountdownTimer(0.2f);
            Range = 300f;

            AddAnimation(0, "base", 4);
            SetAnimation("base");
        }
    }
}
