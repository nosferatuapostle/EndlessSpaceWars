using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles;
using System;
using System.Collections.Generic;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;

namespace EndlessSpace
{
    public class Unit : AnimatedObject, ICollisionActor, IUnitValueOwner
    {
        const float CRITICAL_MULTIPLIER = 2.0f;

        public bool is_selected;

        int level;

        protected AnimatedEngine engine;
        protected Weapon weapon;
        protected List<Skill> skill_list = new List<Skill>();

        UnitEvent unit_event;
        protected UnitInfo info;
        protected float[] values_increase;
        protected Dictionary<UnitValue, UnitValueInfo> base_values;

        Movable movement;
        protected EffectTarget effect_target;

        public Unit(string[] path, Vector2 position, Vector2 size, UnitFaction faction, List<Unit> unit_list) : base(path, position, size)
        {
            level = 1;
            unit_event = new UnitEvent();

            base_values = new Dictionary<UnitValue, UnitValueInfo>
            {
                { UnitValue.Health, new UnitValueInfo(UnitValue.Health, 10f) },
                { UnitValue.Heal, new UnitValueInfo(UnitValue.Heal, 0.0f) },
                { UnitValue.HealRate, new UnitValueInfo(UnitValue.HealRate, 1f, 2f) },
                { UnitValue.CriticalChance, new UnitValueInfo(UnitValue.CriticalChance, 0f, 1f) },
                { UnitValue.Magnitude, new UnitValueInfo(UnitValue.Magnitude, 1f) },
                { UnitValue.DamageResist, new UnitValueInfo(UnitValue.DamageResist, 1f) },
                { UnitValue.SpeedMult, new UnitValueInfo(UnitValue.SpeedMult, 100f, 200f) }
            };

            values_increase = new float[base_values.Count];

            Faction = faction;

            if (unit_list != null) info = new UnitInfo(this, unit_list);
            
            movement = new Movable(this);
            weapon = new Weapon(this, (owner, target) => new LightningBoltProj(owner.Position, owner, (Unit)target));
            effect_target = new EffectTarget(this);

            unit_event.on_death += OnDeath;
        }

        public UnitEvent Event => unit_event;

        public UnitInfo UnitInfo => info;
        public float[] IncreaseValues => values_increase;
        public Dictionary<UnitValue, UnitValueInfo> Values => base_values;

        public AnimatedEngine Engine => engine;
        public EffectTarget EffectTarget => effect_target;

        public Weapon Weap => weapon;
        public List<Skill> SkillList => skill_list;

        public int Level
        {
            get => level;
            set
            {
                level = value;
            }
        }

        public string Name { get; protected set; } = "None";
        public UnitFaction Faction { get; protected set; } = UnitFaction.None;

        public Unit Target => weapon.Target;
        public virtual void Attack(Unit target, float delta_time)
        {
            if (target == null || target.IsDead) return;
            if (weapon.Range < Vector2.Distance(Position, target.Position))
            {
                MoveTo(target.Position);
            }
            else
            {
                MoveStop();
                Rotate(target.Position, delta_time);
                weapon.PassProjectile(target, delta_time);
            }
        }

        public virtual bool HostileTo(Unit unit)
        {
            if (unit == this) return false;
            if (unit is Character character && character.type is Asteroid) return false;

            return unit.Faction != Faction;
        }

        public bool IsDead { get; protected set; } = false;
        public bool IsDestroyed { get; protected set; } = false;

        public void MoveTo(Vector2 position) => movement.SetTarget(position);
        public void MoveStop() => movement.Stop();
        public bool Movable => movement.IsMovable;

        public void Rotate(Vector2 position, float delta_time)
        {
            const float SPEED = 10f;
            float target_rotation = MathF.Atan2(position.Y - Position.Y, position.X - Position.X) + MathHelper.PiOver2;
            float rotation_difference = MathHelper.WrapAngle(target_rotation - Rotation);
            float max_rotation = SPEED * delta_time;
            if (MathF.Abs(rotation_difference) < max_rotation)
            {
                Rotation = target_rotation;
                return;
            }

            Rotation += MathHelper.Clamp(rotation_difference, -max_rotation, max_rotation);
        }

        public override void Update(GameTime game_time)
        {
            base.Update(game_time);
            
            effect_target.Update(game_time);
            weapon.Update(game_time);
            movement.Update(game_time);
            info?.Update(game_time);
            engine?.Update(game_time);

            if (GetBaseUnitValue(UnitValue.Health) > GetUnitValue(UnitValue.Health) && !IsDead)
            {
                RestoreUnitValue(UnitValue.Health, GetUnitValue(UnitValue.Heal) * GetUnitValue(UnitValue.HealRate) * game_time.GetElapsedSeconds());
            }
        }

        public void OnCollision(CollisionEventArgs collision_info)
        {
            if (collision_info.Other is Unit unit)
            {
                float distance = Vector2.Distance(Position, unit.Position);
                float radius = (unit is Character asteroid && asteroid.type is Asteroid) ? (unit.Size * unit.Scale).X / 4f : unit.Bounds.BoundingRectangle.Width / 4f;

                if (distance == 0f)
                {
                    Position += new Vector2(0.1f, 0.1f);
                }
                else if (unit is Character character && character.IsSpaceStation && !(this as Character).IsSpaceStation)
                {
                    Vector2 scaled = unit.Size * unit.Scale;
                    RectangleF unit_rectangle = new RectangleF(unit.Position - scaled / 2f, scaled);

                    if (Position.X > unit_rectangle.Left && Position.X < unit_rectangle.Right && Position.Y > unit_rectangle.Top && Position.Y < unit_rectangle.Bottom)
                    {
                        Position -= Vector2.Normalize(unit.Position - Position);
                    }
                }
                else
                {
                    if (distance < radius)
                    {
                        float strength = 1f - (distance / radius);
                        var direction = Vector2.Normalize(unit.Position - Position);

                        var push_force = direction * (radius - distance) * strength;

                        Position -= push_force * 0.1f;
                        unit.Position += push_force * 0.1f;
                    }
                }
            }
        }

        public virtual void GetDamage(Unit source, float damage)
        {
            GetDamage(source, damage, Color.White);
        }

        public virtual void GetDamage(Unit source, float damage, Color throb_color)
        {
            effect_target.ActivateThrob(throb_color);

            if (source == null || Faction == UnitFaction.Summoned) return;
            Event.OnAttacked(this, source, ref damage);

            if (damage <= 0f) return;

            float resistance = GetUnitValue(UnitValue.DamageResist);
            float magnitude = source.GetUnitValue(UnitValue.Magnitude);

            if (Random.Shared.NextDouble() < source.GetUnitValue(UnitValue.CriticalChance))
                damage *= CRITICAL_MULTIPLIER;

            float final_damage = damage.Calc(resistance, magnitude);

            if (final_damage > 0.5f)
                info.AddFloatingDamage(final_damage, Color.Red);

            RestoreUnitValue(UnitValue.Health, -final_damage);

            if (GetUnitValue(UnitValue.Health) <= 0)
                Event.OnDeath(this, source);
        }

        protected virtual void OnDeath(Unit dying, Unit killer)
        {
            SetAnimation("death");
            if (killer is PlayerCharacter player && !dying.IsDead && dying != player)
            {
                AddKillReward(player, dying);
            }
            IsDead = true;
        }

        protected void AddKillReward(PlayerCharacter player, Unit dying)
        {
            player.Experience.AddExp(MathF.Max(1f, dying.Level * (dying.Level / player.Level)));
            return;
            if (dying is NPC npc && npc.IsBoss)
            {
            }
        }

        public virtual void Kill()
        {
            SetAnimation("death");
            IsDead = true;
        }

        public virtual ParticleEffect Explosion()
        {
            ParticleEffect explosion_effect = new ParticleEffect
            {
                Position = Position,
                Emitters =
                {
                    new ParticleEmitter(new Texture2DRegion(Particle.Spark), 20, TimeSpan.FromSeconds(1), Profile.Circle(Size.X/4f, Profile.CircleRadiation.Out))
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(25f, 75f),
                            Quantity = 4,
                            Rotation = new Range<float>(-1f, 1f),
                            Scale = new Range<float>(0.5f, 1.5f),
                            Color = Color.LightYellow.ToHsl()
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new OpacityInterpolator
                                    {
                                        StartValue = 1f,
                                        EndValue = 0f
                                    }
                                }
                            }
                        }
                    }
                
                }
            };

            return explosion_effect;
        }

        public float GetUnitValue(UnitValue value)
        {
            return base_values.TryGetValue(value, out var info) ? info.CurrentValue : 0;
        }

        public float GetBaseUnitValue(UnitValue value)
        {
            return base_values.TryGetValue(value, out var info) ? info.BaseValue : 0;
        }

        public void SetBaseUnitValue(UnitValue value, float base_value)
        {
            if (base_values.ContainsKey(value))
            {
                base_values[value] = new UnitValueInfo(value, base_value);
            }
            else
            {
                base_values.Add(value, new UnitValueInfo(value, base_value));
            }
        }

        public void SetUnitValue(UnitValue value, float amount)
        {
            if (base_values.TryGetValue(value, out var info))
            {
                info.SetValue(amount);
            }
        }

        public void ModifyUnitValue(UnitValue value, float amount)
        {
            if (base_values.TryGetValue(value, out var info))
            {
                info.ModifyValue(amount);
            }
        }

        public void RestoreUnitValue(UnitValue value, float amount)
        {
            if (base_values.TryGetValue(value, out var info))
            {
                info.ModifyValue(amount);
            }
        }
    }
}
