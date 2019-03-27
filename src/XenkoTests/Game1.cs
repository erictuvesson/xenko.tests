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

    public class Game1 : GameCore
    {
        private Entity cubeEntity;

        protected override Task<Scene> BuildScene()
        {
            var scene = new Scene();

            scene.Entities.Add(cubeEntity = GetCube());
            scene.Entities.Add(GetLight());

            var cameraEntity = new Entity { new CameraComponent { Slot = Services.GetSafeServiceAs<SceneSystem>().GraphicsCompositor.Cameras[0].ToSlotId() } };
            cameraEntity.Transform.Position = new Vector3(0, 0, 5);
            scene.Entities.Add(cameraEntity);

            return Task.FromResult(scene);
        }

        protected Entity GetCube()
        {
            var cubeEntity = new Entity();

            // Create a procedural model with a diffuse material
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

            return cubeEntity;
        }

        protected Entity GetLight()
        {
            var lightEntity = new Entity()
            {
                new LightComponent()
            };

            lightEntity.Transform.Position = new Vector3(0, 0, 1);
            lightEntity.Transform.Rotation = Quaternion.RotationY(MathUtil.DegreesToRadians(45));

            return lightEntity;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var time = (float)gameTime.Total.TotalSeconds;
            cubeEntity.Transform.Rotation = Quaternion.RotationY(time) * Quaternion.RotationX(time * 0.5f);
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
