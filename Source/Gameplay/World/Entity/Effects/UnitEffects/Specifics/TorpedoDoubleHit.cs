using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class TorpedoDoubleHit : UnitEffect
    {
        Weapon weapon;
        bool was_attack;
        CountdownTimer timer;

        public TorpedoDoubleHit(Unit source, Weapon weapon) : base("Torpedo Double Hit", source, null, 0f, 0f, 0f)
        {
            AddKeyword("specific");

            this.weapon = weapon;
            was_attack = false;

            timer = new CountdownTimer(0.2f);

            source.Event.on_attack += OnAttack;
        }

        void OnAttack(Unit attacker, Unit target)
        {
            if (attacker == source)
            {
                this.target = target;
                was_attack = true;
                timer.Restart();
            }
        }

        public override void Update(GameTime game_time)
        {
            timer.Update(game_time);

            if (was_attack && timer.State == TimerState.Completed)
            {
                was_attack = false;

                if (target == null || target.IsDead) goto skip;

                Projectile projectile = weapon.projectile(source, target);
                EntityManager.PassProjectile(projectile);
            }
            skip:

            base.Update(game_time);
        }

        public override void OnEffectEnd()
        {
            source.Event.on_attack -= OnAttack;
            base.OnEffectEnd();
        }
    }
}
