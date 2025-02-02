using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class Summoned : Unit
    {
        public Summoned(string name, string path, Vector2 position, Vector2 size) : base(new string[] { path }, position, size, UnitFaction.Summoned, null)
        {
            Name = name;

            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 1f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 0f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 0f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 1.1f, 2f) }
            };

            values_increase = new float[] { 0f, 0f, 0f, 0f, 0.25f, 0f, 0.01f };

            projectile = (owner, target) => new Minigun(owner.Position, owner, (Unit)target);

            AddAnimation(0, "idle", 1);
            AddAnimation(0, "death", 1, false);
        }
    }
}
