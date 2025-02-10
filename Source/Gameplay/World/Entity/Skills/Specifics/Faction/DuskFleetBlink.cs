using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class DuskFleetBlink(Unit owner) : Skill("Blink", Tag.Escape, owner, 1f)
    {
        public Vector2 position;

        protected override void Use()
        {
            owner.Rotation = owner.Position.ToAngle(Input.MouseWorld);
            owner.EffectTarget.ActivateThrob(Color.LightBlue);
            if (owner is NPC) owner.Position = position;
            else owner.Position = Input.MouseWorld;
        }
    }
}
