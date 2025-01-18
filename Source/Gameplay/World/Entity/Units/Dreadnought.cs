using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 5f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0.02f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 1.9f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 1f, 2f) }
            };

            values_increase = new float[] { 1f, 0.01f, 0.01f, 0.003f, 0.1f, 0.07f, 0.0015f };

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
