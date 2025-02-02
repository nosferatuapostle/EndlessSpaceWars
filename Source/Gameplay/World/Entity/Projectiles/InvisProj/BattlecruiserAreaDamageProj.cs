using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class BattlecruiserAreaDamageProj : InvisProj
    {
        public BattlecruiserAreaDamageProj(Vector2 position, Unit owner) : base(position, new Vector2(800, 800), owner, null, 5f)
        {
            Name = "Battlecruiser Area Damage";
            damage = 0.4f;
            speed = 0f;
        }

        protected override void HitAction(Unit target)
        {
            if (target.EffectTarget.HasEffect(Name)) return;
            target.EffectTarget.AddEffect(new DamageAura(Name, owner, target, damage, Color.OrangeRed));
        }
    }
}
