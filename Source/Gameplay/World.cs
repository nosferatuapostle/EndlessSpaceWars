using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.ViewportAdapters;

namespace EndlessSpace
{
    public class World
    {
        WindowViewportAdapter viewport_adapter;
        static OrthographicCamera camera;
        CameraController camera_controller;
        PlayerCharacter player;

        CollisionComponent collision_component;
        
        EntityManager entity_manager;
        EffectManager effect_manager;
        
        EncounterZone encounter_zone;

        public World(GraphicsDevice graphics_device, GameWindow window)
        {
            viewport_adapter = new WindowViewportAdapter(window, graphics_device);
            camera = new OrthographicCamera(viewport_adapter);
            camera_controller = new CameraController();

            collision_component = new CollisionComponent(new Layer(new SpatialHash(new Vector2(120, 120))));

            entity_manager = new EntityManager(this, collision_component);
            EntityManager.PassProjectile = entity_manager.AddProjectile;
            EntityManager.PassUnit = entity_manager.AddUnit;

            effect_manager = new EffectManager();
            EffectManager.PassEffect = effect_manager.AddEffect;
            EffectManager.PassLightning = effect_manager.AddLightning;

            Vector2 position = new Vector2(graphics_device.Viewport.Width / 2, graphics_device.Viewport.Height / 2);
            player = new PlayerCharacter(new Scout(position, UnitFaction.IronCorpse), entity_manager.UnitList);
            EntityManager.PassUnit(player);

            encounter_zone = new EncounterZone(player, entity_manager.UnitList);

            NPC station = new NPC(new SpaceStation(pos + new Vector2(1200f, 0f), UnitFaction.IronCorpse), entity_manager.UnitList, 1);
            station.SetBehavior(Behavior.None);
            NPC asteroid = new NPC(new Asteroid(pos + new Vector2(1600f, 0f)), entity_manager.UnitList, 1);
            asteroid.SetBehavior(Behavior.None);
            EntityManager.PassUnit(station);
            EntityManager.PassUnit(asteroid);
        }

        public static OrthographicCamera Camera => camera;
        public EntityManager EntityManager => entity_manager;
        public PlayerCharacter PlayerCharacter => player;

        bool camera_to_player = false;
        public void Update(GameTime game_time)
        {
            EntityManager.UnitsCount = entity_manager.UnitList.Count;

            if (Input.WasKeyPressed(Keys.Space))
            {
                camera_to_player = !camera_to_player;
            }
            if (camera_to_player)
            {
                Camera.LookAt(player.Position);
            }

            camera_controller.Update(game_time);

            entity_manager.Update(game_time);
            collision_component.Update(game_time);

            effect_manager.Update(game_time);

            encounter_zone.Update(game_time);
        }

        public Vector2 pos = new Vector2(-200, -200);

        public void Draw(SpriteBatch sprite_batch)
        {
            Vector2 parallax_offset = new Vector2(0.1f, 0.1f);
            Matrix parallax = camera.GetViewMatrix(parallax_offset);

            Matrix transform = camera.GetViewMatrix();

            /*sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: transform);
            //entity_manager.DrawUnitInfo(sprite_batch);
            sprite_batch.End();*/

            sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicWrap, transformMatrix: transform);
            entity_manager.DrawProjectile(sprite_batch);
            effect_manager.Draw(sprite_batch);
            sprite_batch.End();

            sprite_batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, transformMatrix: transform/*, effect: Shader.AntiAliasing*/);
            
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
