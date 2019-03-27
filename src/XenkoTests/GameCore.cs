namespace XenkoTests
{
    using System.Threading.Tasks;
    using Xenko.Engine;
    using Xenko.Graphics;
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
    }
}
