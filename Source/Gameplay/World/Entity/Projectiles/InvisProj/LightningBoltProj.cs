using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class LightningBoltProj : InvisProj
    {
        LightningBolt lightning_bolt;

        public LightningBoltProj(Vector2 pos, Unit owner, Unit target) : base(pos, new Vector2(25, 25), owner, target)
        {
            Name = "Lightning Bolt";
            damage = 1f;
            speed = 0f;

            Range = 300f;
            Cooldown = new CountdownTimer(1f);

            if (target != null)
            {
                lightning_bolt = new LightningBolt(owner.Position, target.Position, Color.AliceBlue);
                EffectManager.PassLightning(lightning_bolt);
                Position = target.Position;
            }
        }

        protected override bool HitCondition(Unit unit) => unit != owner && !unit.IsDead;

        public override void ProjPosition(GameTime game_time) { }
    }
}
