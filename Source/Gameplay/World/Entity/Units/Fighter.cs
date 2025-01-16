using Microsoft.Xna.Framework;

namespace EndlessSpace
{
    public class Fighter : Unit
    {
        public Fighter(Vector2 position, UnitFaction faction) : base(new string[] {
                "Textures\\Unit\\Fighter_00_04",
                "Textures\\Unit\\Fighter_00_01",
                "Textures\\Unit\\Fighter_01_04",
                "Textures\\Unit\\Fighter_01_01",
                "Textures\\Unit\\Fighter_02_04",
                "Textures\\Unit\\Fighter_02_01"
            }, position, new Vector2(64, 64), faction, null)
        {
            Name = "Fighter";

            switch (faction)
            {
                case UnitFaction.Biomantes:
                    AddAnimation(0, "attack", 9);
                    AddAnimation(0, "idle", 1);
                    AddAnimation(1, "death", 3, false);
                    engine = new AnimatedEngine(this, new string[] {"Textures\\Unit\\Fighter_00_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.DuskFleet:
                    AddAnimation(2, "attack", 28);
                    AddAnimation(2, "idle", 1);
                    AddAnimation(3, "death", 17, false);
                    engine = new AnimatedEngine(this, new string[] {"Textures\\Unit\\Fighter_01_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.IronCorpse:
                    AddAnimation(4, "attack", 6);
                    AddAnimation(4, "idle", 1);
                    AddAnimation(5, "death", 3, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Fighter_02_02" }, Vector2.Zero, 10);
                    break;
            }
        }
    }
}
