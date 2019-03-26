﻿namespace XenkoTests
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
    using Xenko.UI;
    using Xenko.UI.Controls;
    using Xenko.UI.Panels;

    public class Game1 : Game
    {
        private SpriteFont arial16;
        private Entity cubeEntity;

        protected async override Task LoadContent()
        {
            await base.LoadContent();

            Window.AllowUserResizing = true;

            // Instantiate a scene with a single entity and model component
            var scene = new Scene();

            arial16 = Content.Load<SpriteFont>("Arial18");

            scene.Entities.Add(GetUIEntity(arial16, true, new Vector3(0, 0, 4)));

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

        protected Entity GetUIEntity(SpriteFont font, bool fixedSize, Vector3 position)
        {
            // Create and initialize "Touch Screen to Start"
            var touchStartLabel = new ContentDecorator
            {
                Content = new TextBlock
                {
                    Font = font,
                    TextSize = 32,
                    Text = (fixedSize) ? "Fixed Size UI" : "Regular UI",
                    TextColor = Color.White
                },
                Padding = new Thickness(30, 20, 30, 25),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            //touchStartLabel.SetPanelZIndex(1);

            var grid = new Grid
            {
                BackgroundColor = (fixedSize) ? new Color(255, 0, 255) : new Color(255, 255, 0),
                MaximumWidth = 100,
                MaximumHeight = 100,
                MinimumWidth = 100,
                MinimumHeight = 100,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            grid.RowDefinitions.Add(new StripDefinition(StripType.Auto));
            grid.ColumnDefinitions.Add(new StripDefinition());
            grid.LayerDefinitions.Add(new StripDefinition());

            grid.Children.Add(touchStartLabel);

            // Add the background
            var background = new ImageElement { StretchType = StretchType.Fill };
            background.SetPanelZIndex(-1);

            var uiEntity = new Entity();

            // Create a procedural model with a diffuse material
            var uiComponent = new UIComponent
            {
                Page = new UIPage
                {
                    RootElement = new UniformGrid { Children = { background, grid } }
                },
                //IsBillboard = true,
                IsFixedSize = fixedSize,
                IsFullScreen = false,
                Resolution = new Vector3(100, 100, 100), // Same size as the inner grid
                Size = new Vector3(0.1f), // 10% of the vertical resolution
            };
            uiEntity.Add(uiComponent);

            uiEntity.Transform.Position = position;

            return uiEntity;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var time = (float)gameTime.Total.TotalSeconds;
            cubeEntity.Transform.Rotation = Quaternion.RotationY(time) * Quaternion.RotationX(time * 0.5f);
        }
    }
}
