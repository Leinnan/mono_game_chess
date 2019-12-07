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
        private (int,int) screenSize;
        
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
            Console.WriteLine("[MOVEMENT] " + movement.X + " " + movement.Y );
        }
        
        public override void OnLoad(ContentManager content, GraphicsDevice graphics)
        {
            screenSize = (graphics.Viewport.Width,graphics.Viewport.Height);
            Console.WriteLine("OnLoad"); 
            font = content.Load<SpriteFont>("Font"); // Use the name of your sprite font file here instead of 'Score'.
            background = content.Load<Texture2D>("bg");
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            //Console.WriteLine("OnDraw"); 
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(background,new Rectangle(0,0,screenSize.Item1,screenSize.Item2), Color.White);
            spriteBatch.DrawString(font, "Score: " + 10.ToString(), new Vector2(10, 10), Color.White);
        }  
    }
}