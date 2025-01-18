using System;

namespace EndlessSpace
{
    public delegate void AttackedEventHandler(Unit victim, Unit source, ref float damage);

    public class UnitEvent
    {
        public event Action<Unit, Unit> on_attack;
        public event AttackedEventHandler on_attacked;
        public event Action<Unit, Unit> on_death;

        public void OnAttack(Unit attacker, Unit target) => on_attack?.Invoke(attacker, target);
        public void OnAttacked(Unit victim, Unit source, ref float damage) => on_attacked?.Invoke(victim, source, ref damage);
        public void OnDeath(Unit dying, Unit killer) => on_death?.Invoke(dying, killer);
    }
}
