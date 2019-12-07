using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameAndroid1
{
    public class Board
    {
        private Piece[] pieces = new Piece[Consts.PIECES_PER_PLAYER*2];
        Field[,] fields = new Field[Consts.BOARD_SIZE,Consts.BOARD_SIZE];
        public Vector2i StartPos { get; set; }

        public Board()
        {
            for (var i = 0; i < fields.GetLength(0); i++)
            for (var j = 0; j < fields.GetLength(1); j++)
                fields[i, j] = new Field(i, j);

            pieces[0] = new Piece(0, 0, Piece.PieceColor.White);
            pieces[1] = new Piece(2, 0, Piece.PieceColor.White);
            pieces[2] = new Piece(4, 0, Piece.PieceColor.White);
            pieces[3] = new Piece(6, 0, Piece.PieceColor.White);
            pieces[4] = new Piece(1, 1, Piece.PieceColor.White);
            pieces[5] = new Piece(3, 1, Piece.PieceColor.White);
            pieces[6] = new Piece(5, 1, Piece.PieceColor.White);
            pieces[7] = new Piece(7, 1, Piece.PieceColor.White);
            pieces[8] = new Piece(0, 2, Piece.PieceColor.White);
            pieces[9] = new Piece(2, 2, Piece.PieceColor.White);
            pieces[10] = new Piece(4, 2, Piece.PieceColor.White);
            pieces[11] = new Piece(6, 2, Piece.PieceColor.White);
            // teraz ciemne
            pieces[12] = new Piece(1, 7, Piece.PieceColor.Black);
            pieces[13] = new Piece(3, 7, Piece.PieceColor.Black);
            pieces[14] = new Piece(5, 7, Piece.PieceColor.Black);
            pieces[15] = new Piece(7, 7, Piece.PieceColor.Black);
            pieces[16] = new Piece(0, 6, Piece.PieceColor.Black);
            pieces[17] = new Piece(2, 6, Piece.PieceColor.Black);
            pieces[18] = new Piece(4, 6, Piece.PieceColor.Black);
            pieces[19] = new Piece(6, 6, Piece.PieceColor.Black);
            pieces[20] = new Piece(1, 5, Piece.PieceColor.Black);
            pieces[21] = new Piece(3, 5, Piece.PieceColor.Black);
            pieces[22] = new Piece(5, 5, Piece.PieceColor.Black);
            pieces[23] = new Piece(7, 5, Piece.PieceColor.Black);
        }
        
        public bool IsAnyPieceOnPos(int x,int y)
        {
            for (var i = 0; i < pieces.Length; i++)
            {
                if (pieces[i].pos.x == x && pieces[i].pos.y == y)
                    return true;
            }

            return false;
        }

        public void UpdatePosibleMoves(int x, int y)
        {
            if (!IsAnyPieceOnPos(x, y))
            {
                return;
            }
            if (x > 0 && y > 0)
            {
                if (!IsAnyPieceOnPos(x - 1, y - 1) && fields[x - 1, y - 1].isDark)
                {
                    Console.WriteLine($"Possible move on {x-1}x{y-1}");
                }
            }
            if (x > 0)
            {
                if (!IsAnyPieceOnPos(x - 1, y) && fields[x - 1, y].isDark)
                {
                    Console.WriteLine($"Possible move on {x-1}x{y}");
                }
            }
            if (x > 0 && y < (Consts.BOARD_SIZE - 1))
            {
                if (!IsAnyPieceOnPos(x - 1, y + 1) && fields[x - 1, y + 1].isDark)
                {
                    Console.WriteLine($"Possible move on {x-1}x{y+1}");
                }
            }
            if (y < (Consts.BOARD_SIZE - 1))
            {
                if (!IsAnyPieceOnPos(x, y + 1) && fields[x, y + 1].isDark)
                {
                    Console.WriteLine($"Possible move on {x}x{y+1}");
                }
            }
            if (x < (Consts.BOARD_SIZE - 1) && y < (Consts.BOARD_SIZE - 1))
            {
                if (!IsAnyPieceOnPos(x+1, y + 1) && fields[x + 1, y + 1].isDark)
                {
                    Console.WriteLine($"Possible move on {x+1}x{y+1}");
                }
            }
            if (x < (Consts.BOARD_SIZE - 1))
            {
                if (!IsAnyPieceOnPos(x+1, y ) && fields[x + 1, y].isDark)
                {
                    Console.WriteLine($"Possible move on {x+1}x{y}");
                }
            }
            if (x < (Consts.BOARD_SIZE - 1) && y > 0)
            {
                if (!IsAnyPieceOnPos(x+1, y - 1) && fields[x + 1, y - 1].isDark)
                {
                    Console.WriteLine($"Possible move on {x+1}x{y-1}");
                }
            }
            if ( y > 0)
            {
                if (!IsAnyPieceOnPos(x, y - 1) && fields[x, y - 1].isDark)
                {
                    Console.WriteLine($"Possible move on {x}x{y-1}");
                }
            }
        }
        
        public void BoardClicked(Vector2 pos)
        {
            var size = new Vector2i(80, 80);
            for (var i = 0; i < fields.GetLength(0); i++)
            {
                for (var j = 0; j < fields.GetLength(1); j++)
                {
                    var field = fields[i, j];
                    var startPos = new Vector2(StartPos.x + (i * size.x), StartPos.y + (j * size.y));
                    var diff = pos - startPos;
                    if (diff.X > 0 && diff.X < size.x && diff.Y > 0 && diff.Y < size.y)
                    {
                        Console.WriteLine($"Input on field {i}x{j}");
                        UpdatePosibleMoves(i,j);
                    }
                }
            }
        }

        public void Draw(ref SpriteBatch spriteBatch, Texture2D fieldTexture, Texture2D pieceTexture)
        {
            var size = new Vector2i(80, 80);
            for (var i = 0; i < fields.GetLength(0); i++)
            {
                for (var j = 0; j < fields.GetLength(1); j++)
                {
                    var field = fields[i, j];
                    spriteBatch.Draw(fieldTexture,
                        new Rectangle(StartPos.x + (i * size.x), StartPos.y + (j * size.y), size.x,
                            size.y), field.isDark ? Color.Gray : Color.White);
                }
            }
            
            Vector2i margin = new Vector2i(8,8);
            for (var i = 0; i < pieces.Length; i++)
            {
                var piece = pieces[i];
                spriteBatch.Draw(pieceTexture,
                    new Rectangle(StartPos.x + (piece.pos.x * size.x) + margin.x, StartPos.y + (piece.pos.y * size.y)+margin.y, size.x - margin.x*2,
                        size.y-margin.y*2), piece.color == Piece.PieceColor.Black ? Color.DarkGray : Color.White);
            }
        }
    }
}