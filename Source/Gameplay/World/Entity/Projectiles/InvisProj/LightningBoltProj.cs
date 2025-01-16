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
            tick_time = new CountdownTimer(0.01f);

            Range = 300f;
            Cooldown = new CountdownTimer(1f);

            if (target != null)
            {
                lightning_bolt = new LightningBolt(owner.Position, target.Position, Color.AliceBlue);
                EffectManager.PassLightning(lightning_bolt);
                Position = target.Position;
            }

        }

        protected override void OnHit(Unit target, Color color)
        {
            base.OnHit(target, Color.White);
        }

        public override void ProjPosition(GameTime game_time) { }
    }
}
