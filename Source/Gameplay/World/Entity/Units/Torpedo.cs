using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 4f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0.02f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 0.8f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 0.3f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 1.05f, 2f) }
            };

            values_increase = new float[] { 0.7f, 0.01f, 0.01f, 0.0075f, 0.2f, 0.036f, 0.007f };

            projectile = (owner, target) => new Deleter(owner.Position, owner, (Unit)target);


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
