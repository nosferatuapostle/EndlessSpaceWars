using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class BattlecruiserAura : UnitEffect
    {
        BattlecruiserAuraProj projectile;
        
        public BattlecruiserAura(Unit source) : base("Battlecruiser Aura", source, null, 0.25f, 0f, 0f)
        {
            ActivateAura();
        }

        private void ActivateAura()
        {
            if (projectile != null) projectile.on_destroy -= ActivateAura;
            projectile = new BattlecruiserAuraProj(source.Position, source);
            projectile.on_destroy += ActivateAura;
            EntityManager.PassProjectile(projectile);
        }

        public override void Update(GameTime game_time)
        {
            projectile.Position = source.Position;
            base.Update(game_time);
        }

        public override void OnEffectEnd()
        {
            if (projectile != null)
            {
                projectile.on_destroy -= ActivateAura;
                projectile.Done();
            }
            base.OnEffectEnd();
        }
    }
}
