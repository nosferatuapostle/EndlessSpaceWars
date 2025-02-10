using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class SupportAura : UnitEffect
    {
        SupportAuraProj projectile;

        public SupportAura(Unit source) : base("Support Aura", source, null, 0f, 0f, 0f)
        {
            AddKeyword("specific");

            ActivateAura();
        }

        void ActivateAura()
        {
            if (projectile != null) projectile.on_destroy -= ActivateAura;
            projectile = new SupportAuraProj(source.Position, source);
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
