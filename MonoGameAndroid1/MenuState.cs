using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameAndroid1
{
    public class MenuState : State
    {
        public override string Id => "MenuState";

        private SpriteFont font;
        private Texture2D background;
        private (int, int) screenSize;
        private Button newGameButton;

        public override void OnInit()
        {
        }

        public override void OnShutdown()
        {
        }

        public override void OnUpdate(GameTime gameTime)
        {
        }

        public override void OnMouseReleased(Vector2 mousePos, Vector2 movement)
        {
            base.OnMouseReleased(mousePos, movement);
            if (newGameButton.IsPosOverRect(mousePos))
            {
                RequestState("GameState");
            }
        }

        public override void OnEnter()
        {
        }

        public override void OnQuit()
        {
        }

        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            screenSize = (graphics.Viewport.Width, graphics.Viewport.Height);
            font = content.Load<SpriteFont>("Font");
            background = content.Load<Texture2D>("bgMenu");
            Texture2D btnTxt = content.Load<Texture2D>("btn");
            newGameButton = new Button(btnTxt, font, "new game", new Rectangle((screenSize.Item1 - 360) / 2, 400, 360, 150));
        }

        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {
        }

        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(-50, 0, screenSize.Item1 + 100, screenSize.Item2), Color.White);
            newGameButton.Draw(ref spriteBatch);
        }
    }
}