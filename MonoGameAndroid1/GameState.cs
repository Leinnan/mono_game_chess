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
        private Texture2D pieceQueenTexture;
        private Texture2D emptyTexture;
        private (int,int) screenSize;
        private Board board = new Board();
        
        public GameState(){}

        public override void OnInit()
        {
            Console.WriteLine("OnInit"); 
            board.StartGame();
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
            pieceQueenTexture = content.Load<Texture2D>("pieceQueen");
            background = content.Load<Texture2D>("bg");
            emptyTexture = new Texture2D(graphics, 1, 1);
            emptyTexture.SetData(new Color[]{Color.White});
        }
        public override void OnUnload(ContentManager content, GraphicsDevice graphics)
        {

        }
        public override void OnDraw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background,new Rectangle(-50,0,screenSize.Item1+100,screenSize.Item2), Color.White);
            
            var fields = board.Fields;
            var startPos = board.StartPos;
            var pieces = board.AlivePieces;
            var possibleMoves = board.PossibleMoves;
            var moveablePieces = board.PiecesWithMoves;
            var size = new Vector2i(80, 80);
            Vector2i margin = new Vector2i(8,8);
            
            foreach (var field in fields)
            {
                spriteBatch.Draw(fieldTexture,
                    new Rectangle(startPos.x + (field.pos.x * size.x), startPos.y + (field.pos.y * size.y), size.x,
                        size.y), field.isDark ? Color.Gray : Color.White);
            }

            foreach (var moveablePiece in moveablePieces)
            {
                spriteBatch.Draw(fieldTexture,
                    new Rectangle(startPos.x + (moveablePiece.x * size.x), startPos.y + (moveablePiece.y * size.y), size.x,
                        size.y), Color.Orange);
            }

            if (board.AnyFieldSelected)
            {
                spriteBatch.Draw(fieldTexture,
                    new Rectangle(startPos.x + (board.SelectedField.x * size.x), startPos.y + (board.SelectedField.y * size.y), size.x,
                        size.y), Color.GreenYellow);
                foreach (var move in possibleMoves)
                {
                    spriteBatch.Draw(fieldTexture,
                        new Rectangle(startPos.x + (move.EndPos.x * size.x), startPos.y + (move.EndPos.y * size.y), size.x,
                            size.y), Color.LightYellow);
                }
            }

            foreach (var piece in pieces)
            {
                spriteBatch.Draw(piece.isQueen ? pieceQueenTexture : pieceTexture,
                    new Rectangle(startPos.x + (piece.pos.x * size.x) + margin.x, startPos.y + (piece.pos.y * size.y)+margin.y, size.x - margin.x*2,
                        size.y-margin.y*2), piece.color == Piece.PieceColor.Black ? Color.DarkGray : Color.White);
            }
            //spriteBatch.DrawString(font, "Score: " + 10.ToString(), new Vector2(10, 10), Color.White);
        }  
    }
}