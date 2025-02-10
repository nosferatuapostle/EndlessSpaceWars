using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class EntityManager
    {
        public static PassObject PassUnit, PassProjectile;

        public static int UnitsCount = 0;

        const float MAX_DISTANCE = 4000f;

        World world;
        CollisionComponent collision_component;

        List<Unit> unit_list = new List<Unit>();
        List<Projectile> projectile_list = new List<Projectile>();

        List<Unit> pending_units = new List<Unit>();
        List<Projectile> pending_projectiles = new List<Projectile>();

        List<Explosion> explosion_list = new List<Explosion>();

        Color shader_color;

        public EntityManager(World world, CollisionComponent collision_component)
        {
            this.world = world;
            this.collision_component = collision_component;

            shader_color = Color.White;
        }

        public List<Projectile> Projectiles => projectile_list;
        public List<Unit> Units => unit_list;

        public void AddProjectile(object info) => pending_projectiles.Add((Projectile)info);

        public void AddUnit(object info) => pending_units.Add((Unit)info);

        public void ProjectileRemover()
        {
            projectile_list.RemoveRange(0, projectile_list.Count);
            for (int i = 0; i < projectile_list.Count; i++) collision_component.Remove(projectile_list[i]);
        }

        public void UnitRemover()
        {
            unit_list.RemoveRange(0, unit_list.Count);
            for (int i = 0; i < unit_list.Count; i++) collision_component.Remove(unit_list[i]);
        }

        public void UnitRemover(Unit unit)
        {
            unit_list.Remove(unit);
            collision_component.Remove(unit);
        }

        public void Update(GameTime game_time)
        {
            UnitsCount = Units.Count;

            for (int i = 0; i < pending_projectiles.Count; i++)
            {
                projectile_list.Add(pending_projectiles[i]);
                collision_component.Insert(pending_projectiles[i]);
            }
            pending_projectiles.Clear();

            for (int i = 0; i < pending_units.Count; i++)
            {
                unit_list.Add(pending_units[i]);
                collision_component.Insert(pending_units[i]);
            }
            pending_units.Clear();

            for (int i = unit_list.Count - 1; i >= 0; i--)
            {
                unit_list[i].Update(game_time);
                float distance_to_player = Vector2.Distance(unit_list[i].Position, world.PlayerCharacter.Position);
                float distance_to_camera = Vector2.Distance(unit_list[i].Position, World.Camera.Center.ToPoint().ToVector2());
                if ((distance_to_camera > MAX_DISTANCE && distance_to_player > MAX_DISTANCE) || unit_list[i].IsDestroyed)
                {
                    if (unit_list[i].Explosion() == null) goto skip;
                    Explosion explosion = new Explosion(unit_list[i].Position, unit_list[i].Explosion(), 1f);
                    explosion_list.Add(explosion);
                    
                    skip:
                    collision_component.Remove(unit_list[i]);
                    unit_list.RemoveAt(i);
                    continue;
                }
            }

            for (int i = projectile_list.Count - 1; i >= 0; i--)
            {
                projectile_list[i].Update(game_time);

                if (projectile_list[i].IsDone)
                {
                    if (projectile_list[i].Explosion() == null) goto skip;
                    Explosion explosion = new Explosion(projectile_list[i].Position, projectile_list[i].Explosion(), 1f);
                    explosion_list.Add(explosion);
                    
                    skip:
                    projectile_list[i].OnDestroy();
                    collision_component.Remove(projectile_list[i]);
                    projectile_list.RemoveAt(i);
                    continue;
                }
            }

            for (int i = explosion_list.Count - 1; i >= 0; i--)
            {
                explosion_list[i].Update(game_time);
                if (explosion_list[i].IsComplited)
                {
                    explosion_list.RemoveAt(i);
                }
            }
        }

        public void DrawUnit(SpriteBatch sprite_batch)
        {
            foreach (Unit unit in unit_list)
            {
                if (unit.IsHoveredInWorld() || unit.is_selected)
                {
                    if (unit.is_selected)
                    {
                        shader_color = Color.White;
                    }
                    else if (unit.HostileTo(world.PlayerCharacter))
                    {
                        shader_color = Color.Red;
                    }
                    else if ((unit as Character).IsPlayerTeammate)
                    {
                        shader_color = Color.Green;
                    }
                    else
                    {
                        shader_color = Color.Blue;
                    }

                    if (GameGlobals.SelectCondition(unit))
                    {
                        sprite_batch.DrawRectangle(unit.GetRectangle(), shader_color * 0.5f);
                    }
                }

                if (unit.IsDead || !GameGlobals.ShaderCondition(unit))
                {
                    unit.Draw(sprite_batch);
                }

                if (unit.Movable) unit.Engine?.Draw(sprite_batch);
                unit.UnitInfo?.Draw(sprite_batch);
            }

            foreach (Unit unit in unit_list)
            {
                if (!unit.IsDead)
                {
                    if (!GameGlobals.SelectCondition(unit))
                    {
                        if (unit.IsHoveredInWorld() || unit.is_selected)
                        {
                            float outline_thickness = 2.8f / unit.Size.X / unit.FrameCount;
                            Shader.Outline.Parameters["OutlineColor"].SetValue(shader_color.ToVector4());
                            Shader.Outline.Parameters["OutlineThickness"].SetValue(outline_thickness);
                            Shader.Outline.CurrentTechnique.Passes[0].Apply();
                            unit.Draw(sprite_batch);
                        }
                    }

                    if (unit.EffectTarget.IsThrob)
                    {
                        float sin_value = (float)Math.Sin((unit.EffectTarget.ThrobTimer.CurrentTime.TotalMilliseconds / unit.EffectTarget.ThrobTimer.Interval.TotalMilliseconds + Math.PI / 2) * (MathF.PI * 3));
                        Shader.Throb.Parameters["SINLOC"].SetValue(sin_value/2);
                        Shader.Throb.Parameters["filter_color"].SetValue(unit.EffectTarget.ThrobColor.ToVector4());
                        Shader.Throb.CurrentTechnique.Passes[0].Apply();
                        unit.Draw(sprite_batch);
                    }

                    if (unit.HasKeyword("stone_armor") && !unit.HasKeyword("invisible"))
                    {
                        Shader.GrayScale.CurrentTechnique.Passes[0].Apply();
                        unit.Draw(sprite_batch);
                    }
                }
            }
        }

        public void DrawProjectile(SpriteBatch sprite_batch)
        {
            foreach (Projectile projectile in projectile_list)
            {
                projectile.Draw(sprite_batch);

                //sprite_batch.DrawCircle((CircleF)projectile.Bounds, 64, Color.Red * 0.75f);
                //sprite_batch.DrawCircle(new CircleF(projectile.Position, projectile.Radius), 64, Color.Green * 0.75f);
            }
        }

        public void DrawEffects(SpriteBatch sprite_batch)
        {
            //sprite_batch.DrawCircle(new CircleF(World.Camera.Center.ToPoint().ToVector2(), NPC.RADIUS/2f), 64, Color.White);

            foreach (Unit unit in unit_list)
            {
                unit.EffectTarget.Draw(sprite_batch);
            }

            foreach (Explosion explosion in explosion_list)
            {
                explosion.Draw(sprite_batch);
            }
        }
    }
}
