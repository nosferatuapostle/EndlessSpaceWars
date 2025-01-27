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
        
        static ulong next_id = 0;
        public ulong ID { get; private set; } = 0;

        public Character(Unit unit, List<Unit> unit_list) : base(unit.GetPath, unit.Position, unit.Size, unit.Faction, unit_list)
        {
            ID = ++next_id;

            type = unit;
            IsSpaceStation = unit is SpaceStation;
            Name = unit.Name;
            Scale = unit.Scale;

            base_values = unit.Values;

            engine = unit.Engine;
            unit.Engine?.SetUnit(this);

            //weapon = unit.Weap;
            skill_list = unit.SkillList;

            if (unit_list != null) info = new UnitInfo(this, unit_list);

            ApplySpecific(unit, unit_list);

            CopyAnimations(unit.GetAnimations);
            SetAnimation("attack");
            if (animated_sprite == null) SetAnimation("idle");

            animations["death"].Controller.OnAnimationEvent += (s, t) =>
            {
                if (t == AnimationEventTrigger.AnimationCompleted)
                {
                    IsDestroyed = true;
                }
            };
        }

        public bool IsSpaceStation { get; private set; }

        public bool IsPlayerTeammate { get; protected set; } = false;

        public static void ResetNextID()
        {
            UnitEffect.ResetID();
            next_id = 0;
        }

        public void ApplySpecific(Unit unit, List<Unit> unit_list)
        {
            switch (unit)
            {
                case Battlecruiser:
                    EffectTarget.AddEffect(new BattlecruiserAura(this, unit_list));
                    break;
                case Frigate:
                    EffectTarget.AddEffect(new FrigateDamageReflect(this));
                    break;
                case Bomber:
                    EffectTarget.AddEffect(new BomberHitModifier(this, weapon));
                    break;
                case Dreadnought:
                    EffectTarget.AddEffect(new DreadnoughtCommander(this, unit_list));
                    break;
                case Scout:
                    EffectTarget.AddEffect(new ScoutEvasion(this));
                    break;
                case Torpedo:
                    EffectTarget.AddEffect(new TorpedoDoubleHit(this, weapon));
                    break;
                case Support:
                    EffectTarget.AddEffect(new SupportAura(this, unit_list));
                    break;
            }
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
            Vector2 scaled = Size * Scale;
            if (Faction == UnitFaction.Summoned || Name == "Space Station") return new RectangleF(Position - scaled / 2f, scaled);
            if (type is Asteroid) return new RectangleF(Position - scaled / 4f, scaled / 2f);
            return base.GetRectangle();
        }
    }
}
