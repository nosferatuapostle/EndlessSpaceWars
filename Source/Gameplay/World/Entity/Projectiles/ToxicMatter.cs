using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class ToxicMatter : Projectile
    {
        public ToxicMatter(Vector2 position, Unit owner, Unit target) : base(new string[] { "Textures\\Projectile\\ToxicMatter" }, position, new Vector2(8, 8), owner, target)
        {
            Name = "Toxic Matter";
            damage = 1f;
            speed = 200f;

            Cooldown = new CountdownTimer(1f);
            Range = 300f;

            AddAnimation(0, "base", 8);
            SetAnimation("base");
        }
    }
}
