using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class SpaceStation : Unit, IObjective
    {
        static int index;
        string[] path = new string[] { "Textures\\Unit\\Objective\\space_station_00", "Textures\\Unit\\Objective\\space_station_01", "Textures\\Unit\\Objective\\space_station_02" };
        
        public SpaceStation(Vector2 position, UnitFaction faction) : base(null, position, Vector2.Zero, faction, null)
        {
            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 10f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0.02f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 0.48f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 0f, 2f) }
            };

            values_increase = new float[] { 2.5f, 0.01f, 0.01f, 0f, 0.125f, 0.08f, 0f };

            Name = "Space Station";
            Size = InitData(faction);
            Bounds = new CircleF(Position, Size.X * 4f);

            atlas = new Texture2DAtlas[path.Length];
            sheets = new SpriteSheet[atlas.Length];

            atlas[index] = Texture2DAtlas.Create(null, Globals.Content.Load<Texture2D>(path[index]), (int)Size.X, (int)Size.Y);
            sheets[index] = new SpriteSheet(null, atlas[index]);

            AddAnimation(index, "idle", 1);
            AddAnimation(index, "death", 1, false);
        }

        private Vector2 InitData(UnitFaction faction)
        {
            switch (faction)
            {
                case UnitFaction.Biomantes:
                    index = 0;
                    return new Vector2(166f, 315f);
                case UnitFaction.DuskFleet:
                    index = 1;
                    return new Vector2(126f, 345f);
                case UnitFaction.IronCorpse:
                    index = 2;
                    return new Vector2(185f, 320f);
                default:
                    return Vector2.Zero;
            }
        }
    }
}
