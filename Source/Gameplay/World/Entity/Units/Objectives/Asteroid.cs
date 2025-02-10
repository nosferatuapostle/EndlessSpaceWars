using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class Asteroid : Unit, IObjective
    {
        public Asteroid(Vector2 position) : base(new string[] { "Textures\\Unit\\Objective\\asteroid_00", "Textures\\Unit\\Objective\\asteroid_01", "Textures\\Unit\\Objective\\asteroid_02" }, position, new Vector2(64, 64), UnitFaction.None, null)
        {
            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 5f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 0.01f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 0f, 2f) }
            };

            values_increase = new float[] { 1f, 0f, 0f, 0f, 0f, 0.01f, 0f };

            Name = "Asteroid";
            float scale_multiplier = (float)Globals.Random.NextSingle(0.5f, 1.25f);
            Scale *= scale_multiplier;

            int index = Globals.Random.Next(0, 3);
            AddAnimation(index, "idle", 1);
            AddAnimation(index, "death", 1, false);
        }
    }
}
