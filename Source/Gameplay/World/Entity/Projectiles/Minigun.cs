using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class Minigun : Projectile
    {
        public Minigun(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\Minigun" }, position, new Vector2(4, 16), owner, target)
        {
            Name = "Minigun";
            damage = 0.25f;
            speed = 300f;

            Cooldown = new CountdownTimer(0.2f);
            Range = 200f;

            AddAnimation(0, "base", 4);
            SetAnimation("base");
        }
    }
}
