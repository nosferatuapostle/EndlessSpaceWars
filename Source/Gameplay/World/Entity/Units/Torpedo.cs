using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Torpedo : Unit
    {
        public Torpedo(Vector2 position, UnitFaction faction) : base(new string[] {
                "Textures\\Unit\\Torpedo_00_04",
                "Textures\\Unit\\Torpedo_00_01",
                "Textures\\Unit\\Torpedo_01_04",
                "Textures\\Unit\\Torpedo_01_01",
                "Textures\\Unit\\Torpedo_02_04",
                "Textures\\Unit\\Torpedo_02_01"
            }, position, new Vector2(64, 64), faction, null)
        {
            Name = "Torpedo";

            switch (faction)
            {
                case UnitFaction.Biomantes:
                    AddAnimation(0, "attack", 16);
                    AddAnimation(0, "idle", 1);
                    AddAnimation(1, "death", 8, false);
                    engine = new AnimatedEngine(this, new string[] {"Textures\\Unit\\Torpedo_00_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.DuskFleet:
                    AddAnimation(2, "attack", 12);
                    AddAnimation(2, "idle", 1);
                    AddAnimation(3, "death", 16, false);
                    engine = new AnimatedEngine(this, new string[] {"Textures\\Unit\\Torpedo_01_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.IronCorpse:
                    AddAnimation(4, "attack", 16);
                    AddAnimation(4, "idle", 1);
                    AddAnimation(5, "death", 4, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Torpedo_02_02" }, Vector2.Zero, 10);
                    break;
            }
        }
    }
}
