using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class Pulsator : Projectile
    {
        public Pulsator(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\Pulsator" }, position, new Vector2(32, 32), owner, target)
        {
            Name = "Pulsator";
            damage = 1f;
            speed = 200f;

            Cooldown = new CountdownTimer(1f);
            Range = 300f;

            AddAnimation(0, "base", 10);
            SetAnimation("base");
        }
    }
}
