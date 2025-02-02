using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class Cutter : Projectile
    {
        public Cutter(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\Cutter" }, position, new Vector2(9, 9), owner, target)
        {
            Name = "Cutter";
            damage = 1f;
            speed = 200f;

            Cooldown = new CountdownTimer(1f);
            Range = 300f;

            AddAnimation(0, "base", 5);
            SetAnimation("base");
        }
    }
}
