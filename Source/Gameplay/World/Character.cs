using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using System.Collections.Generic;
using System.Linq;

namespace EndlessSpace
{
    public class Character : Unit
    {
        public Unit type;

        public Skill current_skill;
        public readonly HashSet<Skill> used_skills;

        static ulong next_id = 1;
        public ulong ID { get; protected set; } = 0;

        public Character(Unit unit, List<Unit> unit_list) : base(unit.GetPath, unit.Position, unit.Size, unit.Faction, unit_list)
        {
            ID = ++next_id;

            used_skills = new HashSet<Skill>();

            SetUnitType(unit, unit_list);

            animations["death"].Controller.OnAnimationEvent += (s, t) =>
            {
                if (t == AnimationEventTrigger.AnimationCompleted)
                {
                    IsDestroyed = true;
                }
            };
        }

        public bool IsPlayerTeammate { get; protected set; } = false;

        public static void ResetNextID()
        {
            UnitEffect.ResetID();
            next_id = 0;
        }

        public void SetUnitType(Unit unit, List<Unit> unit_list)
        {
            type = unit;
            Name = unit.Name;
            Scale = unit.Scale;

            base_values = unit.Values;

            engine = unit.Engine;
            unit.Engine?.SetUnit(this);
            skill_list = unit.Skills;

            if (unit_list != null) info = new UnitInfo(this, unit_list);

            weapon.SwitchProjectile(unit.Projectile);
            ApplySpecific(unit, unit_list);

            //if (animated_sprite != null) animated_sprite.Controller.Stop();
            CopyAnimations(unit.GetAnimations);
            SetAnimation("attack");
            if (animated_sprite == null) SetAnimation("idle");
        }

        public override void Update(GameTime game_time)
        {
            used_skills.RemoveWhere(skill => skill == null || skill.IsReady);
            foreach (var skill in used_skills) skill.Update(game_time);
            base.Update(game_time);
        }

        public void CheckInvisable()
        {
            if (HasKeyword("invisible")) animated_sprite.Alpha = 0.3f;
            else animated_sprite.Alpha = 1f;
        }

        public void ApplySpecific(Unit unit, List<Unit> unit_list)
        {
            for (int i = 0; i < effect_target.ActiveEffects.Count; i++)
            {
                if (!effect_target.ActiveEffects[i].HasKeyword("specific")) continue;
                effect_target.RemoveEffect(effect_target.ActiveEffects[i]);
            }

            switch (unit)
            {
                case Battlecruiser:
                    EffectTarget.AddEffect(new BattlecruiserAura(this));
                    skill_list.Add(new BattlecruiserAreaDamage(this));
                    break;
                case Bomber:
                    EffectTarget.AddEffect(new BomberHitModifier(this, weapon));
                    skill_list.Add(new BomberBomb(this));
                    break;
                case Dreadnought:
                    EffectTarget.AddEffect(new DreadnoughtCommander(this, unit_list));
                    skill_list.Add(new DreadnoughtCommandedUnit(this, unit_list));
                    break;
                case Fighter:
                    EffectTarget.AddEffect(new FighterCorrosion(this, weapon));
                    skill_list.Add(new FighterExtraDamage(this, weapon));
                    break;
                case Frigate:
                    EffectTarget.AddEffect(new FrigateDamageReflect(this));
                    skill_list.Add(new FrigateStoneArmor(this));
                    break;
                case Scout:
                    EffectTarget.AddEffect(new ScoutEvasion(this));
                    skill_list.Add(new ScoutDash(this));
                    break;
                case Support:
                    EffectTarget.AddEffect(new SupportAura(this));
                    skill_list.Add(new SupportHealCircle(this));
                    break;
                case Torpedo:
                    EffectTarget.AddEffect(new TorpedoDoubleHit(this, weapon));
                    skill_list.Add(new TorpedoSpeedUp(this));
                    break;
                case SpaceStation:
                    skill_list.Add(new SpaceStationRepair(this));
                    skill_list.Add(new Summon(this, unit_list));
                    AddKeyword("space_station");
                    break;
                case Asteroid:
                    AddKeyword("asteroid");
                    return;
            }

            switch (unit.Faction)
            {
                case UnitFaction.Biomantes:
                    skill_list.Add(new BiomantesInvisability(this));
                    break;
                case UnitFaction.DuskFleet:
                    skill_list.Add(new DuskFleetBlink(this));
                    break;
                case UnitFaction.IronCorpse:
                    skill_list.Add(new IronCorpseOverload(this));
                    break;
            }
            if (this is PlayerCharacter) skill_list.Add(new Summon(this, unit_list));
        }

        public void UpdateStats(int init_level = 1)
        {
            for (int i = 0; i < type.IncreaseValues.Length; i++)
            {
                var element = type.Values.ElementAt(i);
                var base_value = GetBaseUnitValue(element.Key);
                SetBaseUnitValue(element.Key, base_value + type.IncreaseValues[i] * init_level);
            }
        }

        public override RectangleF GetRectangle()
        {
            if (Faction == UnitFaction.Summoned || HasKeyword("space_station")) return new RectangleF(Position - Size / 2f, Size);
            if (type is Asteroid) return new RectangleF(Position - Size / 4f, Size / 2f);
            return base.GetRectangle();
        }
    }
}
