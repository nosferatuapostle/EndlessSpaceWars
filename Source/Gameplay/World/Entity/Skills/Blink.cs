using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Blink(Unit owner) : Skill("Blink", Tag.Escape, owner, 0f)
    {
        protected override void Use()
        {
            owner.Rotation = owner.Position.ToAngle(Input.MouseWorld);
            owner.EffectTarget.ActivateThrob(Color.LightBlue);
            owner.Position = Input.MouseWorld;
        }
    }
}
