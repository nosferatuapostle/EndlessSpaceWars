using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Bomber : Unit
    {
        public Bomber(Vector2 position, UnitFaction faction) : base(new string[] {
                "Textures\\Unit\\Bomber_00_00",
                "Textures\\Unit\\Bomber_00_01",
                "Textures\\Unit\\Bomber_01_00",
                "Textures\\Unit\\Bomber_01_01",
                "Textures\\Unit\\Bomber_02_00",
                "Textures\\Unit\\Bomber_02_01"
            }, position, new Vector2(64, 64), faction, null)
        {
            Name = "Bomber";

            switch (faction)
            {
                case UnitFaction.Biomantes:
                    AddAnimation(0, "idle", 1);
                    AddAnimation(1, "death", 10, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Bomber_00_02" }, Vector2.Zero, 8);
                    break;
                case UnitFaction.DuskFleet:
                    AddAnimation(2, "idle", 1);
                    AddAnimation(3, "death", 16, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Bomber_01_02" }, Vector2.Zero, 8);
                    break;
                case UnitFaction.IronCorpse:
                    AddAnimation(4, "idle", 1);
                    AddAnimation(5, "death", 3, false);
                    engine = new AnimatedEngine(this, new string[] {"Textures\\Unit\\Bomber_02_02"}, Vector2.Zero, 10);
                    break;
            }
        }
    }
}
