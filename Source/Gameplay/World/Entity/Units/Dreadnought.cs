using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Dreadnought : Unit
    {
        public Dreadnought(Vector2 position, UnitFaction faction) : base(new string[] {
                "Textures\\Unit\\Dreadnought_00_04",
                "Textures\\Unit\\Dreadnought_00_01",
                "Textures\\Unit\\Dreadnought_01_04",
                "Textures\\Unit\\Dreadnought_01_01",
                "Textures\\Unit\\Dreadnought_02_04",
                "Textures\\Unit\\Dreadnought_02_01"
            }, position, new Vector2(128, 128), faction, null)
        {
            Name = "Dreadnought";

            switch (faction)
            {
                case UnitFaction.Biomantes:
                    AddAnimation(0, "attack", 35);
                    AddAnimation(0, "idle", 1);
                    AddAnimation(1, "death", 12, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Dreadnought_00_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.DuskFleet:
                    AddAnimation(2, "attack", 34);
                    AddAnimation(2, "idle", 1);
                    AddAnimation(3, "death", 18, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Dreadnought_01_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.IronCorpse:
                    AddAnimation(4, "attack", 60);
                    AddAnimation(4, "idle", 1);
                    AddAnimation(5, "death", 12, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Dreadnought_02_02" }, Vector2.Zero, 12);
                    break;
            }
        }
    }
}
