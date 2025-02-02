using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace EndlessSpace
{
    public class BomberProj : InvisProj
    {
        public BomberProj(Vector2 position, Unit owner, Unit target) : base(position, new Vector2(25, 25), owner, target)
        {
            Name = "Bomber Modifier";
            damage = 10f;
            speed = 0f;
        }

        public override void ProjPosition(GameTime game_time) { }
    }
}
