using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameAndroid1
{
    public class Board
    {
        static readonly Vector2i WRONG_POS = new Vector2i(-1,-1);
        private Piece[] pieces = new Piece[Consts.PIECES_PER_PLAYER*2];
        private Vector2i selectedField = WRONG_POS;
        private List<Move> possibleMoves = new List<Move>();
        Field[] fields = new Field[Consts.BOARD_SIZE * Consts.BOARD_SIZE];
        public Vector2i StartPos { get; set; }
        public Field[] Fields => fields;
        public Piece[] AlivePieces => pieces.Where(piece => piece.Alive).ToArray();

        public Vector2i SelectedField => selectedField;

        public List<Move> PossibleMoves => possibleMoves;

        public bool AnyFieldSelected
        {
            get => SelectedField != WRONG_POS;
            private set => selectedField = value ? selectedField : WRONG_POS;
        }
        public Piece.PieceColor ActiveColor { get; set; }
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

            StartGame();
        }

        void StartGame()
        {
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
            ActiveColor = Piece.PieceColor.White;
            AnyFieldSelected = false;
        }
        
        bool IsAnyPieceOnPos(Vector2i pos)
        {
            return AlivePieces.Any(piece => piece.pos == pos);
        }

        Piece.PieceColor PieceColorOnPos(int x, int y)
        {
            for (var i = 0; i < pieces.Length; i++)
            {
                if (pieces[i].pos.x == x && pieces[i].pos.y == y)
                    return pieces[i].color;
            }

            return Piece.PieceColor.None;
        }
        
        bool CanMovePieceToPos(int x,int y)
        {
            if (x < 0 || x > Consts.BOARD_SIZE || y < 0 || y > Consts.BOARD_SIZE)
                return false;

            if (!FieldAtPos(x, y).isDark)
                return false;
            
            for (var i = 0; i < pieces.Length; i++)
            {
                if (pieces[i].pos.x == x && pieces[i].pos.y == y)
                    return false;
            }

            return true;
        }
        void CheckPosForMove(Piece piece, int x, int y)
        {
            var newPos = new Vector2i(x,y);
            var moveCheck = piece.pos.y < y ? piece.MovesDown : piece.MovesUp;
            
            if (moveCheck && CanMovePieceToPos(newPos.x, newPos.y))
            {
                Console.WriteLine($"Possible move on {newPos.x}x{newPos.y}");
                possibleMoves.Add(new Move(selectedField, newPos));
            }
        }

        void UpdatePosibleMoves(int x, int y)
        {
            var pos = new Vector2i(x, y);
            if (!IsAnyPieceOnPos(pos))
            {
                var move = possibleMoves.First(move1 => move1.EndPos == pos);
                if (move != null)
                {
                    MakeMove(move);
                }
                return;
            }

            var piece = PieceOnPos(pos);

            if (piece.color != ActiveColor)
            {
                return;
            }
            selectedField.x = x;
            selectedField.y = y;
            possibleMoves.Clear();
            
            CheckPosForMove(piece, x-1, y-1);
            CheckPosForMove(piece, x+1, y-1);
            CheckPosForMove(piece, x-1, y+1);
            CheckPosForMove(piece, x+1, y+1);
        }

        private Piece PieceOnPos(Vector2i pos)
        {
            foreach (var piece in pieces)
            {
                if (piece.pos == pos)
                    return piece;
            }

            return null;
        }

        private void MakeMove(Move move)
        {
            var piece = PieceOnPos(move.StartPos);
            piece.MoveToPos(move.EndPos);
            foreach (var posToRemovePiece in move.PosToRemovePieces)
            {
                var pieceToRemove = PieceOnPos(posToRemovePiece);
                pieceToRemove.Kill();
            }
            ChangePlayer();
        }

        public void BoardClicked(Vector2 pos)
        {
            var size = new Vector2i(80, 80);
            foreach (var field in fields)
            {
                var startPos = new Vector2(StartPos.x + (field.pos.x * size.x), StartPos.y + (field.pos.y * size.y));
                var diff = pos - startPos;
                
                if (!(diff.X > 0) || !(diff.X < size.x) || !(diff.Y > 0) || !(diff.Y < size.y))
                    continue;
                
                Console.WriteLine($"Input on field {field.pos.x}x{field.pos.y}");
                UpdatePosibleMoves(field.pos.x,field.pos.y);
            }
        }

        void ChangePlayer()
        {
            AnyFieldSelected = false;
            possibleMoves.Clear();
            ActiveColor = ActiveColor == Piece.PieceColor.White ? Piece.PieceColor.Black : Piece.PieceColor.White;
        }
        
        Field FieldAtPos(int x, int y)
        {
            return fields.FirstOrDefault(field => field.pos.x == x && field.pos.y == y);
        }
    }
}