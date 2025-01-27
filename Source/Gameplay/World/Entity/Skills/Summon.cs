using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class Summon : Skill
    {
        protected int level, total_count;
        protected Unit summon;

        List<Unit> unit_list, summon_list;

        public Summon(Unit summoner, List<Unit> unit_list) : base("Summon", summoner)
        {
            this.unit_list = unit_list;
            summon_list = new List<Unit>();
        }

        public override void Update()
        {
            base.Update();
            if (owner.IsDead)
            {
                foreach (Unit unit in summon_list)
                {
                    unit.Kill();
                }
                return;
            }
        }

        protected override void Use()
        {
            total_count = owner.Level / 5 + 1;
            if (summon_list.Count >= total_count)
            {
                summon_list[0].Kill();
                summon_list.RemoveAt(0);
            }
            summon = Summoned();
            EntityManager.PassUnit(summon);
            summon_list.Add(summon);
        }

        private NPC Summoned()
        {
            float distance = owner.Size.X;
            float rotation = owner.Rotation;

            float x_offset = distance * MathF.Cos(rotation - MathF.PI / 2f);
            float y_offset = distance * MathF.Sin(rotation - MathF.PI / 2f);

            Vector2 position = owner.Position + new Vector2(x_offset, y_offset);

            switch (owner.Level)
            {
                case 1:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_00", position, new Vector2(24, 22)), unit_list, owner.Level, owner);
                case 2:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_01", position, new Vector2(34, 27)), unit_list, owner.Level, owner);
                case 3:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_02", position, new Vector2(26, 23)), unit_list, owner.Level, owner);
                case 4:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_03", position, new Vector2(28, 22)), unit_list, owner.Level, owner);
                case 5:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_04", position, new Vector2(32, 27)), unit_list, owner.Level, owner);
                case 6:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_05", position, new Vector2(40, 22)), unit_list, owner.Level, owner);
                case 7:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_06", position, new Vector2(40, 20)), unit_list, owner.Level, owner);
                case 8:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_07", position, new Vector2(32, 22)), unit_list, owner.Level, owner);
                case 9:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_08", position, new Vector2(34, 29)), unit_list, owner.Level, owner);
                case 10:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_09", position, new Vector2(40, 28)), unit_list, owner.Level, owner);
                case 11:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_10", position, new Vector2(36, 26)), unit_list, owner.Level, owner);
                case 12:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_11", position, new Vector2(26, 22)), unit_list, owner.Level, owner);
                case 13:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_12", position, new Vector2(36, 32)), unit_list, owner.Level, owner);
                case 14:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_13", position, new Vector2(52, 30)), unit_list, owner.Level, owner);
                case 15:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_14", position, new Vector2(36, 25)), unit_list, owner.Level, owner);
                case 16:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_15", position, new Vector2(28, 23)), unit_list, owner.Level, owner);
                case 17:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_16", position, new Vector2(34, 24)), unit_list, owner.Level, owner);
                case 18:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_17", position, new Vector2(32, 27)), unit_list, owner.Level, owner);
                case 19:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_18", position, new Vector2(42, 27)), unit_list, owner.Level, owner);
                case 20:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_19", position, new Vector2(44, 36)), unit_list, owner.Level, owner);
                default:
                    return new NPC(new Summoned(Name, "Textures\\Unit\\Summoned\\Summoned_00", position, new Vector2(24, 22)), unit_list, owner.Level, owner);
            }
        }
    }
}
