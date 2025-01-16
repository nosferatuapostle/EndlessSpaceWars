using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Support : Unit
    {
        public Support(Vector2 position, UnitFaction faction) : base(new string[] {
                "Textures\\Unit\\Support_00_00",
                "Textures\\Unit\\Support_00_01",
                "Textures\\Unit\\Support_01_00",
                "Textures\\Unit\\Support_01_01",
                "Textures\\Unit\\Support_02_00",
                "Textures\\Unit\\Support_02_01"
            }, position, new Vector2(64, 64), faction, null)
        {
            Name = "Support";

            switch (faction)
            {
                case UnitFaction.Biomantes:
                    AddAnimation(0, "idle", 1);
                    AddAnimation(1, "death", 8, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Support_00_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.DuskFleet:
                    AddAnimation(2, "idle", 1);
                    AddAnimation(3, "death", 16, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Support_01_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.IronCorpse:
                    AddAnimation(4, "idle", 1);
                    AddAnimation(5, "death", 4, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Support_02_02" }, Vector2.Zero, 10);
                    break;
            }
        }
    }
}
