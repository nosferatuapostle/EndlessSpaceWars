using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.ViewportAdapters;
using System.Diagnostics;
using System;

namespace EndlessSpace
{
    public class World
    {
        public Texture2D texture;

        static OrthographicCamera camera;

        WindowViewportAdapter viewport_adapter;

        CollisionComponent collision_component;

        PlayerCharacter player;

        EntityManager entity_manager;

        EffectManager effect_manager;

        EncounterZone encounter_zone;

        public World(GraphicsDevice graphics_device, GameWindow window)
        {
            texture = Globals.Content.Load<Texture2D>("Textures\\Unit\\Bomber_00_00");

            viewport_adapter = new WindowViewportAdapter(window, graphics_device);
            camera = new OrthographicCamera(viewport_adapter);

            collision_component = new CollisionComponent(new Layer(new SpatialHash(new Vector2(120, 120))));

            entity_manager = new EntityManager(this, collision_component);
            EntityManager.PassProjectile = entity_manager.AddProjectile;
            EntityManager.PassUnit = entity_manager.AddUnit;

            effect_manager = new EffectManager();
            EffectManager.PassEffect = effect_manager.AddEffect;
            EffectManager.PassLightning = effect_manager.AddLightning;

            player = new PlayerCharacter(new Scout(new Vector2(graphics_device.Viewport.Width / 2, graphics_device.Viewport.Height / 2), UnitFaction.Biomantes), entity_manager.UnitList);
            entity_manager.AddUnit(player);

            encounter_zone = new EncounterZone(player, entity_manager.UnitList);
        }

        public static OrthographicCamera Camera => camera;
        public EntityManager EntityManager => entity_manager;
        public PlayerCharacter PlayerCharacter => player;

        public void Update(GameTime game_time)
        {
            EntityManager.UnitsCount = entity_manager.UnitList.Count;

            /*if (Input.WasKeyPressed(Keys.E))
            {
                player.EffectTarget.AddEffect(new AuraBattlecruiser(player, EntityManager.UnitList));
                player.GetDamage(player, 1f);
            }*/

            entity_manager.Update(game_time);
            collision_component.Update(game_time);

            effect_manager.Update(game_time);

            encounter_zone.Update(game_time);
        }

        public Vector2 pos = new Vector2(-200, -200);

        public void Draw(SpriteBatch sprite_batch)
        {
            Matrix transform = camera.GetViewMatrix();

            /*sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: transform);
            //entity_manager.DrawUnitInfo(sprite_batch);
            sprite_batch.End();*/

            sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicWrap, transformMatrix: transform);
            entity_manager.DrawProj(sprite_batch);
            effect_manager.Draw(sprite_batch);
            sprite_batch.End();

            sprite_batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, transformMatrix: transform);
            entity_manager.DrawUnit(sprite_batch);
            sprite_batch.End();

            sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicWrap, transformMatrix: transform);
            entity_manager.DrawEffects(sprite_batch);
            sprite_batch.End();

            /*sprite_batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, effect: Shader.Outline, transformMatrix: transform);
            sprite_batch.Draw(texture, pos, Color.White);
            sprite_batch.End();*/

            /*sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.Additive, transformMatrix: transform);
            entity_manager.DrawCircle(sprite_batch);
            sprite_batch.End();*/
        }
    }
}
