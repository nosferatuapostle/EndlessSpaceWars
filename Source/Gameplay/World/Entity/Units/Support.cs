using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 4f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0.02f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 0.48f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 1.04f, 2f) }
            };

            values_increase = new float[] { 0.75f, 0.01f, 0.01f, 0.0035f, 0.15f, 0.05f, 0.005f };

            projectile = (owner, target) => new Pulsator(owner.Position, owner, (Unit)target);

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
