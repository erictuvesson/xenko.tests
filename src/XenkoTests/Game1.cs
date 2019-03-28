namespace XenkoTests
{
    using System.Threading.Tasks;
    using Xenko.Core;
    using Xenko.Core.Mathematics;
    using Xenko.Engine;
    using Xenko.Games;
    using Xenko.Rendering;
    using Xenko.Rendering.Materials;
    using Xenko.Rendering.Materials.ComputeColors;
    using Xenko.Rendering.ProceduralModels;
    using XenkoTests.Components.Camera;

    public class Game1 : GameCore
    {
        private TopDownCamera camera;

        protected override Task<Scene> BuildScene()
        {
            var scene = new Scene();

            scene.Entities.Add(GetCube(new Vector3(0, 0, 0), new Vector3(15, 1, 15)));
            scene.Entities.Add(GetLight());

            camera = new TopDownCamera(
                    new TopDownCameraSettings(),
                    Services.GetSafeServiceAs<SceneSystem>().GraphicsCompositor.Cameras[0].ToSlotId());

            scene.Entities.Add(new Entity { camera });

            return Task.FromResult(scene);
        }

        protected Entity GetCube(Vector3 position, Vector3? scale = null)
        {
            var cubeEntity = new Entity();

            var model = new Model();
            var material = Material.New(GraphicsDevice, new MaterialDescriptor
            {
                Attributes =
                {
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor(Color.White)),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature()
                }
            });
            model.Materials.Add(material);
            cubeEntity.Add(new ModelComponent(model));

            var modelDescriptor = new ProceduralModelDescriptor(new CubeProceduralModel());
            modelDescriptor.GenerateModel(Services, model);

            cubeEntity.Transform.Position = position;
            cubeEntity.Transform.Scale = scale ?? Vector3.One;

            return cubeEntity;
        }

        protected Entity GetLight()
        {
            var lightEntity = new Entity()
            {
                new LightComponent()
            };

            lightEntity.Transform.Position = new Vector3(0, 20, 0);
            lightEntity.Transform.Rotation = Quaternion.RotationYawPitchRoll(45, -45, 0);

            return lightEntity;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(GraphicsContext);
            SpriteBatch.DrawString(Arial18, "Hello World", new Vector2(50, 50), Color.White);
            SpriteBatch.End();
        }
    }
}
