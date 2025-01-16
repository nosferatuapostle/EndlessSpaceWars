using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Scout : Unit
    {
        public Scout(Vector2 position, UnitFaction faction) : base(new string[] {
                "Textures\\Unit\\Scout_00_04",
                "Textures\\Unit\\Scout_00_01",
                "Textures\\Unit\\Scout_01_04",
                "Textures\\Unit\\Scout_01_01",
                "Textures\\Unit\\Scout_02_04",
                "Textures\\Unit\\Scout_02_01"
            }, position, new Vector2(64, 64), faction, null)
        {
            Name = "Scout";

            switch (faction)
            {
                case UnitFaction.Biomantes:
                    AddAnimation(0, "attack", 7);
                    AddAnimation(0, "idle", 1);
                    AddAnimation(1, "death", 9, false);
                    engine = new AnimatedEngine(this, new string[]{"Textures\\Unit\\Scout_00_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.DuskFleet:
                    AddAnimation(2, "attack", 6);
                    AddAnimation(2, "idle", 1);
                    AddAnimation(3, "death", 16, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Scout_01_02" }, Vector2.Zero, 8);
                    break;
                case UnitFaction.IronCorpse:
                    AddAnimation(4, "attack", 6);
                    AddAnimation(4, "idle", 1);
                    AddAnimation(5, "death", 4, false);
                    engine = new AnimatedEngine(this, new string[]{ "Textures\\Unit\\Scout_02_02" }, Vector2.Zero, 10);
                    break;
            }
        }
    }
}
