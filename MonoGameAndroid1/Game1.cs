using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameAndroid1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateHandler m_stateHandler;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            var graphicsDeviceManager = (GraphicsDeviceManager) Services.GetService(typeof(IGraphicsDeviceManager));
            graphics.PreferredBackBufferWidth = graphicsDeviceManager.PreferredBackBufferWidth;
            graphics.PreferredBackBufferHeight = graphicsDeviceManager.PreferredBackBufferHeight;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            m_stateHandler = new StateHandler();
            m_stateHandler.RegisterState(new GameState());
            m_stateHandler.RegisterState(new MenuState());
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            m_stateHandler.LoadAssets(Content, GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            m_stateHandler.Start("MenuState");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            m_stateHandler.Update(gameTime);
            if (m_stateHandler.IsRequestingGameExit())
                Exit();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            m_stateHandler.Draw(ref spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}