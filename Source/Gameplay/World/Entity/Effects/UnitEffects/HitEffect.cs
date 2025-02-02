
namespace EndlessSpace
{
    public abstract class HitEffect : UnitEffect
    {
        protected Weapon weapon;

        public HitEffect(string name, Unit source, Weapon weapon, float duration = 0f) : base(name, source, null, 0f, duration, 0f)
        {
            this.weapon = weapon;
            var original_projectile = weapon.projectile;
            weapon.projectile = (owner, target) =>
            {
                var proj = original_projectile(owner, target);
                proj.on_hit += target_unit => Activate(target_unit);
                return proj;
            };
        }

        public abstract void Activate(Unit target);
    }
}
