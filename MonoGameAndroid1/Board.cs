using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameAndroid1
{
    public class Board
    {
        private Piece[] pieces = new Piece[Consts.PIECES_PER_PLAYER*2];
        private Vector2i selectedField = new Vector2i(-1,-1);
        private List<Vector2i> possibleMoves = new List<Vector2i>();
        Field[] fields = new Field[Consts.BOARD_SIZE * Consts.BOARD_SIZE];
        public Vector2i StartPos { get; set; }
        public Field[] Fields => fields;
        public Piece[] Pieces => pieces;

        public Vector2i SelectedField => selectedField;

        public Board()
        {
            for (int y = 0,index = 0; y < Consts.BOARD_SIZE; y++)
            {
                for (int x = 0; x < Consts.BOARD_SIZE ; x++)
                {
                    fields[index] = new Field(x, y);
                    index++;
                }
            }

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
        
        public bool CanMovePieceToPos(int x,int y)
        {
            if (x < 0 || x > Consts.BOARD_SIZE || y < 0 || y > Consts.BOARD_SIZE)
                return false;
            
            for (var i = 0; i < pieces.Length; i++)
            {
                if (pieces[i].pos.x == x && pieces[i].pos.y == y)
                    return false;
            }

            return true;
        }

        public void UpdatePosibleMoves(int x, int y)
        {
            if (!IsAnyPieceOnPos(x, y))
            {
                return;
            }

            selectedField.x = x;
            selectedField.y = y;
            possibleMoves.Clear();
            if (x > 0 && y > 0)
            {
                if (CanMovePieceToPos(x - 1, y - 1) && FieldAtPos(x - 1, y - 1).isDark)
                {
                    Console.WriteLine($"Possible move on {x-1}x{y-1}");
                    possibleMoves.Add(new Vector2i(x-1,y-1));
                }
            }
            if (x > 0)
            {
                if (CanMovePieceToPos(x - 1, y) && FieldAtPos(x - 1, y).isDark)
                {
                    Console.WriteLine($"Possible move on {x-1}x{y}");
                    possibleMoves.Add(new Vector2i(x-1,y));
                }
            }
            if (x > 0 && y < (Consts.BOARD_SIZE - 1))
            {
                if (CanMovePieceToPos(x - 1, y + 1) && FieldAtPos(x - 1, y + 1).isDark)
                {
                    Console.WriteLine($"Possible move on {x-1}x{y+1}");
                    possibleMoves.Add(new Vector2i(x-1,y+1));
                }
            }
            if (y < (Consts.BOARD_SIZE - 1))
            {
                if (CanMovePieceToPos(x, y + 1) && FieldAtPos(x, y + 1).isDark)
                {
                    Console.WriteLine($"Possible move on {x}x{y+1}");
                    possibleMoves.Add(new Vector2i(x,y+1));
                }
            }
            if (x < (Consts.BOARD_SIZE - 1) && y < (Consts.BOARD_SIZE - 1))
            {
                if (CanMovePieceToPos(x+1, y + 1) && FieldAtPos(x + 1, y + 1).isDark)
                {
                    Console.WriteLine($"Possible move on {x+1}x{y+1}");
                    possibleMoves.Add(new Vector2i(x+1,y+1));
                }
            }
            if (x < (Consts.BOARD_SIZE - 1))
            {
                if (CanMovePieceToPos(x+1, y ) && FieldAtPos(x + 1, y).isDark)
                {
                    Console.WriteLine($"Possible move on {x+1}x{y}");
                    possibleMoves.Add(new Vector2i(x+1,y));
                }
            }
            if (x < (Consts.BOARD_SIZE - 1) && y > 0)
            {
                if (CanMovePieceToPos(x+1, y - 1) && FieldAtPos(x + 1, y - 1).isDark)
                {
                    Console.WriteLine($"Possible move on {x+1}x{y-1}");
                    possibleMoves.Add(new Vector2i(x+1,y-1));
                }
            }
            if ( y > 0)
            {
                if (CanMovePieceToPos(x, y - 1) && FieldAtPos(x, y - 1).isDark)
                {
                    Console.WriteLine($"Possible move on {x}x{y-1}");
                    possibleMoves.Add(new Vector2i(x,y-1));
                }
            }
        }
        
        public void BoardClicked(Vector2 pos)
        {
            var size = new Vector2i(80, 80);
            foreach (var field in fields)
            {
                var startPos = new Vector2(StartPos.x + (field.pos.x * size.x), StartPos.y + (field.pos.y * size.y));
                var diff = pos - startPos;
                if (diff.X > 0 && diff.X < size.x && diff.Y > 0 && diff.Y < size.y)
                {
                    Console.WriteLine($"Input on field {field.pos.x}x{field.pos.y}");
                    UpdatePosibleMoves(field.pos.x,field.pos.y);
                }
            }
        }
        
        Field FieldAtPos(int x, int y)
        {
            return fields.FirstOrDefault(field => field.pos.x == x && field.pos.y == y);
        }
    }
}