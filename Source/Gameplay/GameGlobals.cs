using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public static class GameGlobals
    {
        public static bool UnAttackable(Unit unit)
        {
            return unit.HasKeyword("invisible") || unit.Faction == UnitFaction.Summoned;
        }

        public static bool SelectCondition(Unit unit)
        {
            return unit.HasKeyword(new string[] { "invisible", "space_station" }) || unit.Faction == UnitFaction.Summoned;
        }

        public static bool ShaderCondition(Unit unit)
        {
            return (!GameGlobals.SelectCondition(unit) && (unit.IsHovered() || unit.is_selected)) || unit.HasKeyword("stone_armor") || unit.EffectTarget.IsThrob;
        }

        public static NPC CommandedNPC(Unit owner, List<Unit> unit_list)
        {
            float distance = owner.Size.X;
            float rotation = owner.Rotation;

            float x_offset = distance * MathF.Cos(rotation - MathF.PI / 2f);
            float y_offset = distance * MathF.Sin(rotation - MathF.PI / 2f);

            Vector2 position = owner.Position + new Vector2(x_offset, y_offset);

            switch (owner.Level)
            {
                case 1:
                    return new NPC(new Scout(owner.Position + new Vector2(x_offset, y_offset), owner.Faction), unit_list, owner.Level, owner);
                case 2:
                    return new NPC(new Fighter(owner.Position + new Vector2(x_offset, y_offset), owner.Faction), unit_list, owner.Level, owner);
                case 3:
                    return new NPC(new Frigate(owner.Position + new Vector2(x_offset, y_offset), owner.Faction), unit_list, owner.Level, owner);
                case 4:
                    return new NPC(new Torpedo(owner.Position + new Vector2(x_offset, y_offset), owner.Faction), unit_list, owner.Level, owner);
                case 5:
                    return new NPC(new Support(owner.Position + new Vector2(x_offset, y_offset), owner.Faction), unit_list, owner.Level, owner);
                default:
                    return new NPC(new Scout(owner.Position + new Vector2(x_offset, y_offset), owner.Faction), unit_list, owner.Level, owner);
            }
        }

        public static NPC SummonedNPC(string name, Unit owner, List<Unit> unit_list)
        {
            float distance = owner.Size.X;
            float rotation = owner.Rotation;

            float x_offset = distance * MathF.Cos(rotation - MathF.PI / 2f);
            float y_offset = distance * MathF.Sin(rotation - MathF.PI / 2f);

            Vector2 position = owner.Position + new Vector2(x_offset, y_offset);

            switch (owner.Level)
            {
                case 1:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_00", position, new Vector2(24, 22)), unit_list, owner.Level, owner);
                case 2:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_01", position, new Vector2(34, 27)), unit_list, owner.Level, owner);
                case 3:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_02", position, new Vector2(26, 23)), unit_list, owner.Level, owner);
                case 4:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_03", position, new Vector2(28, 22)), unit_list, owner.Level, owner);
                case 5:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_04", position, new Vector2(32, 27)), unit_list, owner.Level, owner);
                case 6:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_05", position, new Vector2(40, 22)), unit_list, owner.Level, owner);
                case 7:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_06", position, new Vector2(40, 20)), unit_list, owner.Level, owner);
                case 8:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_07", position, new Vector2(32, 22)), unit_list, owner.Level, owner);
                case 9:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_08", position, new Vector2(34, 29)), unit_list, owner.Level, owner);
                case 10:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_09", position, new Vector2(40, 28)), unit_list, owner.Level, owner);
                case 11:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_10", position, new Vector2(36, 26)), unit_list, owner.Level, owner);
                case 12:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_11", position, new Vector2(26, 22)), unit_list, owner.Level, owner);
                case 13:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_12", position, new Vector2(36, 32)), unit_list, owner.Level, owner);
                case 14:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_13", position, new Vector2(52, 30)), unit_list, owner.Level, owner);
                case 15:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_14", position, new Vector2(36, 25)), unit_list, owner.Level, owner);
                case 16:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_15", position, new Vector2(28, 23)), unit_list, owner.Level, owner);
                case 17:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_16", position, new Vector2(34, 24)), unit_list, owner.Level, owner);
                case 18:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_17", position, new Vector2(32, 27)), unit_list, owner.Level, owner);
                case 19:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_18", position, new Vector2(42, 27)), unit_list, owner.Level, owner);
                case 20:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_19", position, new Vector2(44, 36)), unit_list, owner.Level, owner);
                default:
                    return new NPC(new Summoned(name, "Textures\\Unit\\Summoned\\Summoned_00", position, new Vector2(24, 22)), unit_list, owner.Level, owner);
            }
        }
    }
}
