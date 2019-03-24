namespace XenkoTests
{
    using System.Threading.Tasks;
    using Xenko.Core;
    using Xenko.Core.Mathematics;
    using Xenko.Engine;
    using Xenko.Games;
    using Xenko.Graphics;
    using Xenko.Rendering;
    using Xenko.Rendering.Compositing;
    using Xenko.Rendering.Materials;
    using Xenko.Rendering.Materials.ComputeColors;
    using Xenko.Rendering.ProceduralModels;

    public class Game1 : Game
    {
        private Entity cubeEntity;

        protected async override Task LoadContent()
        {
            await base.LoadContent();

            Window.AllowUserResizing = true;

            // Instantiate a scene with a single entity and model component
            var scene = new Scene();

            // Create a cube entity
            cubeEntity = new Entity();

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

            // Add the cube to the scene
            scene.Entities.Add(cubeEntity);

            // Use this graphics compositor
            SceneSystem.GraphicsCompositor = GraphicsCompositorHelper.CreateDefault(false, graphicsProfile: GraphicsProfile.Level_9_1);

            // Create a camera entity and add it to the scene
            var cameraEntity = new Entity { new CameraComponent { Slot = Services.GetSafeServiceAs<SceneSystem>().GraphicsCompositor.Cameras[0].ToSlotId() } };
            cameraEntity.Transform.Position = new Vector3(0, 0, 5);
            scene.Entities.Add(cameraEntity);

            // Create a light
            var lightEntity = new Entity()
            {
                new LightComponent()
            };

            lightEntity.Transform.Position = new Vector3(0, 0, 1);
            lightEntity.Transform.Rotation = Quaternion.RotationY(MathUtil.DegreesToRadians(45));
            scene.Entities.Add(lightEntity);

            // Create a scene instance
            SceneSystem.SceneInstance = new SceneInstance(Services, scene);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var time = (float)gameTime.Total.TotalSeconds;
            cubeEntity.Transform.Rotation = Quaternion.RotationY(time) * Quaternion.RotationX(time * 0.5f);
        }
    }
}
