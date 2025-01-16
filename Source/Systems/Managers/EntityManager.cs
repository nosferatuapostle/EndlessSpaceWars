using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;

namespace EndlessSpace
{
    public class EntityManager
    {
        public static PassObject PassUnit, PassProjectile;

        public static int UnitsCount = 0;

        const float MAX_DISTANCE = 10000f;

        World world;
        CollisionComponent collision_component;

        List<Unit> unit_list = new List<Unit>();
        List<Projectile> projectile_list = new List<Projectile>();

        private List<Unit> pending_units = new List<Unit>();
        private List<Projectile> pending_projectiles = new List<Projectile>();

        List<Explosion> explosions = new List<Explosion>();

        public EntityManager(World world, CollisionComponent collision_component)
        {
            this.world = world;
            this.collision_component = collision_component;
        }

        public List<Projectile> ProjList => projectile_list;
        public List<Unit> UnitList => unit_list;

        public void AddProjectile(object info) => pending_projectiles.Add((Projectile)info);

        public void AddUnit(object info) => pending_units.Add((Unit)info);

        public void ProjRemover()
        {
            projectile_list.RemoveRange(0, projectile_list.Count);
        }

        public void UnitRemover()
        {
            unit_list.RemoveRange(0, unit_list.Count);
        }

        public void UnitRemover(Unit unit)
        {
            unit_list.Remove(unit);
        }

        public void Update(GameTime game_time)
        {
            foreach (var proj in pending_projectiles)
            {
                projectile_list.Add(proj);
                collision_component.Insert(proj);
            }
            pending_projectiles.Clear();

            foreach (var unit in pending_units)
            {
                unit_list.Add(unit);
                collision_component.Insert(unit);
            }
            pending_units.Clear();

            for (int i = unit_list.Count - 1; i >= 0; i--)
            {
                unit_list[i].Update(game_time);

                float distance_to_player = Vector2.Distance(unit_list[i].Position, unit_list[i] is PlayerCharacter ? ((PlayerCharacter)unit_list[i]).Position : unit_list[i].Position);
                if (distance_to_player > MAX_DISTANCE || unit_list[i].IsDestroyed)
                {
                    Explosion explosion = new Explosion(unit_list[i].Position, unit_list[i].Explosion(), 1f);
                    explosions.Add(explosion);

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
                    Explosion explosion = new Explosion(projectile_list[i].Position, projectile_list[i].Explosion(), 1f);
                    explosions.Add(explosion);

                    collision_component.Remove(projectile_list[i]);
                    projectile_list.RemoveAt(i);
                    continue;
                }
            }

            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(game_time);
                if (explosions[i].IsComplited)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        public void DrawUnit(SpriteBatch sprite_batch)
        {
            foreach (Unit unit in unit_list)
            {
                if (!unit.EffectTarget.IsThrob || unit.IsDead)
                {
                    unit.Engine?.Draw(sprite_batch);
                    unit.UnitInfo?.Draw(sprite_batch);
                    unit.Draw(sprite_batch);

                }
                else
                {
                    unit.Engine?.Draw(sprite_batch);
                    unit.UnitInfo?.Draw(sprite_batch);
                }
            }

            foreach (Unit unit in unit_list)
            {
                if ((unit.IsHovered() || unit.is_selected) && !unit.IsDead)
                {
                    Color color = Color.White;
                    if (unit.HostileTo(world.PlayerCharacter))
                    {
                        color = Color.Red;
                    }
                    else if (unit is Character character && character.IsPlayerTeamate)
                    {
                        color = Color.Green;
                    }
                    else
                    {
                        color = Color.Blue;
                    }

                    float outline_thickness = 1.8f / unit.Width / unit.FrameCount;
                    Shader.Outline.Parameters["OutlineColor"].SetValue(color.ToVector4());
                    Shader.Outline.Parameters["OutlineThickness"].SetValue(outline_thickness);
                    Shader.Outline.CurrentTechnique.Passes[0].Apply();

                    /*if (unit.Faction != UnitFaction.Summoned)
                    {
                        float outline_thickness = 1.8f / unit.Width / unit.FrameCount;
                        Shader.Outline.Parameters["OutlineColor"].SetValue(color.ToVector4() * 0.8f);
                        Shader.Outline.Parameters["OutlineThickness"].SetValue(outline_thickness);
                        Shader.Outline.CurrentTechnique.Passes[0].Apply();
                    }
                    else
                    {
                        Shader.Grayscale.CurrentTechnique.Passes[0].Apply();
                    }*/


                    unit.Draw(sprite_batch);
                }

                if (unit.EffectTarget.IsThrob && !unit.IsDead && unit.Faction != UnitFaction.Summoned)
                {
                    float sin_value = (float)Math.Sin((unit.EffectTarget.ThrobTimer.CurrentTime.TotalMilliseconds / unit.EffectTarget.ThrobTimer.Interval.TotalMilliseconds + Math.PI / 2) * (float)(Math.PI * 3));
                    Shader.Throb.Parameters["SINLOC"].SetValue(sin_value);
                    Shader.Throb.Parameters["filter_color"].SetValue(unit.EffectTarget.ThrobColor.ToVector4());
                    Shader.Throb.CurrentTechnique.Passes[0].Apply();

                    unit.Draw(sprite_batch);
                }
            }
        }

        public void DrawProj(SpriteBatch sprite_batch)
        {
            foreach (Projectile proj in projectile_list)
            {
                proj.Draw(sprite_batch);
            }
        }

        public void DrawEffects(SpriteBatch sprite_batch)
        {
            foreach (Unit unit in unit_list)
            {
                unit.EffectTarget.Draw(sprite_batch);
            }

            foreach (Explosion explosion in explosions)
            {
                explosion.Draw(sprite_batch);
            }
        }

        public void DrawCircle(SpriteBatch sprite_batch)
        {
            foreach (Projectile proj in projectile_list)
            {
                sprite_batch.DrawCircle((CircleF)proj.Bounds, 64, Color.Red * 0.75f);
            }
        }
    }
}
