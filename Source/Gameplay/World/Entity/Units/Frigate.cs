using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class Frigate : Unit
    {
        public Frigate(Vector2 position, UnitFaction faction) : base(new string[] {
                "Textures\\Unit\\Frigate_00_04",
                "Textures\\Unit\\Frigate_00_01",
                "Textures\\Unit\\Frigate_01_04",
                "Textures\\Unit\\Frigate_01_01",
                "Textures\\Unit\\Frigate_02_04",
                "Textures\\Unit\\Frigate_02_01"
            }, position, new Vector2(64, 64), faction, null)
        {
            Name = "Frigate";

            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 4f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0.02f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0.1f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 0.6f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 1.02f, 2f) }
            };

            values_increase = new float[] { 0.8f, 0.01f, 0.01f, 0.004f, 0.125f, 0.05f, 0.005f };

            projectile = (owner, target) => new Penetrator(owner.Position, owner, (Unit)target);


            switch (faction)
            {
                case UnitFaction.Biomantes:
                    AddAnimation(0, "attack", 9);
                    AddAnimation(0, "idle", 1);
                    AddAnimation(1, "death", 9, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Frigate_00_02" }, Vector2.Zero, 8);
                    break;
                case UnitFaction.DuskFleet:
                    AddAnimation(2, "attack", 5);
                    AddAnimation(2, "idle", 1);
                    AddAnimation(3, "death", 16, false);
                    engine = new AnimatedEngine(this, new string[] {"Textures\\Unit\\Frigate_01_02"}, Vector2.Zero, 8);
                    break;
                case UnitFaction.IronCorpse:
                    AddAnimation(4, "attack", 6);
                    AddAnimation(4, "idle", 1);
                    AddAnimation(5, "death", 4, false);
                    engine = new AnimatedEngine(this, new string[] { "Textures\\Unit\\Frigate_02_02"}, Vector2.Zero, 12);
                    break;
            }
        }
    }
}
