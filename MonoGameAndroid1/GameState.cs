using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGameAndroid1 {
    public sealed class GameState : State {
        public override string Id => "GameState";

        private SpriteFont font;
        private Texture2D background;
        private Texture2D fieldTexture;
        private Texture2D pieceTexture;
        private (int,int) screenSize;
        private Board board = new Board();
        
        public GameState(){}

        public override void OnInit()
        {
            Console.WriteLine("OnInit"); 
        }
        public override void OnShutdown()
        {
            Console.WriteLine("OnShutdown"); 
        }
        public override void OnUpdate(GameTime gameTime)
        {

        }
        public override void OnEnter()
        {
            Console.WriteLine("OnEnter"); 
        }
        public override void OnQuit()
        {
            Console.WriteLine("OnQuit"); 
        }

        // input handling
        public override void OnKeyPressed (Keys pressedKey) 
        {            
            switch (pressedKey) {
                case Keys.Escape:
                    RequestGameExit();
                    break;
                default:
                    break;
            }
        }

        public override void OnMouseReleased (Vector2 mousePos, Vector2 movement)
        {
            board.BoardClicked(mousePos);
        }
        
        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            screenSize = (graphics.Viewport.Width,graphics.Viewport.Height);
            board.StartPos = new Vector2i((screenSize.Item1 - Consts.BOARD_SIZE * 80)/2,(screenSize.Item2 - Consts.BOARD_SIZE * 80)/2);
            Console.WriteLine("OnLoad"); 
            font = content.Load<SpriteFont>("Font");
            fieldTexture = content.Load<Texture2D>("field");
            pieceTexture = content.Load<Texture2D>("piece");
            background = content.Load<Texture2D>("bg");
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            //Console.WriteLine("OnDraw"); 
            spriteBatch.Draw(background,new Rectangle(-50,0,screenSize.Item1+100,screenSize.Item2), Color.White);
            board.Draw(ref spriteBatch,fieldTexture,pieceTexture);
            spriteBatch.DrawString(font, "Score: " + 10.ToString(), new Vector2(10, 10), Color.White);
        }  
    }
}