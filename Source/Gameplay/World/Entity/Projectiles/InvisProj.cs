using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class InvisProj : Projectile
    {
        public readonly float time;

        public InvisProj(Vector2 position, Vector2 size, Unit owner, object target, float time = 0.2f) : base(null, position, size, owner, target)
        {
            this.time = time;
            life_time = new CountdownTimer(time);
        }

        protected override bool HitCondition(Unit unit) => !unit.IsDead && unit.HostileTo(owner);

        public override void Draw(SpriteBatch sprite_batch) { }
    }
}
