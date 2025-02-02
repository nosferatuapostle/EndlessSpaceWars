using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 3f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0.02f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0.1f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1.05f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 0.28f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 1.05f, 2f) }
            };

            values_increase = new float[] { 0.62f, 0.01f, 0.01f, 0.005f, 0.2f, 0.03f, 0.004f };

            projectile = (owner, target) => new IronBomb(owner.Position, owner, (Unit)target);

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
