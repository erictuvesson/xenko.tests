namespace XenkoTests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Xenko.Engine;
    using Xenko.Games;
    using Xenko.Graphics;
    using Xenko.Input;
    using Xenko.Rendering.Compositing;

    public abstract class GameCore : Game
    {
        protected SpriteBatch SpriteBatch { get; private set; }

        protected SpriteFont Arial18 { get; private set; }

        protected async override Task LoadContent()
        {
            await base.LoadContent();

            Window.AllowUserResizing = true;

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Arial18 = Content.Load<SpriteFont>("DynamicFonts/Arial18");

            SceneSystem.GraphicsCompositor = GraphicsCompositorHelper.CreateDefault(false, graphicsProfile: GraphicsProfile.Level_9_1);
            SceneSystem.SceneInstance = new SceneInstance(Services, await BuildScene());
        }

        protected abstract Task<Scene> BuildScene();

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

#if DEBUG
            if (Input.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
#endif

            if (Input.IsKeyReleased(Keys.F12))
            {
                TakeScreenshot();
            }
        }

        protected void TakeScreenshot(string filename = null)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = $"screenshot-{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.png";
            }

            var fullName = Path.Combine(Directory.GetCurrentDirectory(), filename);
            using (var image = GraphicsDevice.Presenter.BackBuffer.GetDataAsImage(GraphicsContext.CommandList))
            {
                using (var resultFileStream = File.OpenWrite(fullName))
                {
                    image.Save(resultFileStream, ImageFileType.Png);
                }
            }
        }
    }
}
