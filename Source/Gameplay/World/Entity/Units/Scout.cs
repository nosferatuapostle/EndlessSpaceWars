using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 3f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0.02f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0.05f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1.03f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 0.2f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 1.1f, 2f) }
            };

            values_increase = new float[] { 0.5f, 0.01f, 0.01f, 0.007f, 0.3f, 0.025f, 0.006f };

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
