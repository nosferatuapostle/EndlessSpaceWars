using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EndlessSpace
{
    public class PlayerCharacter : Character
    {
        public event Action on_level_up;

        PlayerController controller;
        Experience experience;

        public PlayerCharacter(Unit unit, List<Unit> unit_list) : base(unit, unit_list)
        {
            ID = 1;
            controller = new PlayerController(this, unit_list);

            experience = new Experience(this);

            skill_list.Add(new Heal(this));
            skill_list.Add(new Blink(this));
            skill_list.Add(new Summon(this, unit_list));

            on_level_up += () => UpdateStats();
        }

        public bool IsUnKillable { get; private set; } = true;
        public void UnKillable() => IsUnKillable = !IsUnKillable;

        public Experience Experience => experience;
        public void OnLevelUp() => on_level_up?.Invoke();

        public void SetFaction(UnitFaction faction) => Faction = faction;

        public void SaveData()
        {
            XDocument xml = new XDocument(
                new XElement("Player",
                    new XElement("Level", Level)
                )
            );

            Globals.Save.HandleSaveFormat(xml, "player");
        }

        public void LoadData(Unit unit, List<Unit> unit_list)
        {
            XDocument xml = Globals.Save.LoadFile("player");
            if (xml == null) return;

            XElement root = xml.Element("Player");
            if (root == null) return;

            int level = int.Parse(root.Element("Level")?.Value ?? "1");

            while (Level < level) experience.AddExp(experience.NextXP);

            SetUnitType(unit, unit_list);
        }

        public override void Update(GameTime game_time)
        {
            controller.Update(game_time);
            base.Update(game_time);
        }

        public override void GetDamage(Unit source, float damage, Color throb_color)
        {
            if (IsUnKillable) return;
            base.GetDamage(source, damage, throb_color);
        }
    }
}
