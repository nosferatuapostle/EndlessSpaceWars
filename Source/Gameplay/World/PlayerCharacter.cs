using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class PlayerCharacter : Character
    {
        public event Action on_level_changed;
        
        PlayerController controller;
        Experience experience;

        public PlayerCharacter(Unit unit, List<Unit> unit_list) : base(unit, unit_list)
        {
            controller = new PlayerController(this, unit_list);

            experience = new Experience(this);

            Level = 100;
            SetBaseUnitValue(UnitValue.SpeedMult, 200f);
            SetBaseUnitValue(UnitValue.Magnitude, 1f);
            SetBaseUnitValue(UnitValue.DamageResist, 0f);


            skill_list.Add(new Heal(this));
            skill_list.Add(new Blink(this));
            skill_list.Add(new Summon(this, unit_list));
        }

        public bool IsUnKillable { get; private set; } = true;
        public void UnKillable() => IsUnKillable = !IsUnKillable;

        public Experience Experience => experience;
        public void OnLevelChanged() => on_level_changed?.Invoke();

        public void SetFaction(UnitFaction faction) => Faction = faction;

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
