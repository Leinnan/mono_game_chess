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
        static readonly Vector2i[] directions = 
        {
            new Vector2i(- 1,  - 1), new Vector2i( + 1,  - 1), new Vector2i( - 1,  + 1),
            new Vector2i( + 1,  + 1),
        };
        private Piece[] pieces = new Piece[Consts.PIECES_PER_PLAYER*2];
        private Vector2i selectedField = WRONG_POS;
        private List<Move> allPossibleMoves = new List<Move>();
        Field[] fields = new Field[Consts.BOARD_SIZE * Consts.BOARD_SIZE];
        public Vector2i StartPos { get; set; }
        public Field[] Fields => fields;
        public Piece[] AlivePieces => pieces.Where(piece => piece.Alive).ToArray();
        private bool IsGameEnded => allPossibleMoves.Count == 0;

        public Vector2i SelectedField => selectedField;

        public List<Move> PossibleMoves => AnyFieldSelected ? allPossibleMoves.Where(move => move.StartPos == selectedField).ToList() : new List<Move>();

        public List<Vector2i> PiecesWithMoves => AlivePieces
            .Where(piece => allPossibleMoves.Any(move => move.StartPos == piece.pos)).Select(piece => piece.pos)
            .ToList();
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
            UpdatePosibleMoves();
        }
        
        bool IsAnyPieceOnPos(Vector2i pos)
        {
            return AlivePieces.Any(piece => piece.pos == pos);
        }

        Piece.PieceColor PieceColorOnPos(Vector2i pos)
        {
            for (var i = 0; i < pieces.Length; i++)
            {
                if (pieces[i].Alive && pieces[i].pos == pos)
                    return pieces[i].color;
            }

            return Piece.PieceColor.None;
        }
        
        
        
        bool CanMovePieceToPos(Vector2i pos)
        {
            if (pos.x < 0 || pos.x >= Consts.BOARD_SIZE || pos.y < 0 || pos.y >= Consts.BOARD_SIZE)
                return false;

            if (!FieldAtPos(pos).isDark)
                return false;
            
            for (var i = 0; i < pieces.Length; i++)
            {
                if (pieces[i].pos == pos && pieces[i].Alive)
                    return false;
            }

            return true;
        }
        bool CheckPosForMove(Piece piece, Vector2i pos)
        {
            var moveCheck = piece.pos.y < pos.y ? piece.MovesDown : piece.MovesUp;
            bool canMove = moveCheck && CanMovePieceToPos(pos);
            if (canMove)
            {
                Console.WriteLine($"Possible move on {pos.x}x{pos.y}");
                allPossibleMoves.Add(new Move(piece.pos, pos));
            }

            return canMove;
        }

        void UpdatePosibleMoves()
        {
            allPossibleMoves.Clear();
            var piecesToCheck = pieces.Where(piece => piece.color == ActiveColor && piece.Alive);
            foreach (var piece in piecesToCheck)
            {
                if (piece.isQueen)
                {
                    foreach (var direction in directions)
                    {
                        for (int i = 1; i < Consts.BOARD_SIZE;++i)
                        {
                            var canMove = CheckPosForMove(piece, piece.pos + (direction * i));
                            if (!canMove)
                                break;
                        }
                    }
                }
                else
                {
                    foreach (var direction in directions)
                    {
                        CheckPosForMove(piece, piece.pos+direction);
                    }
                }

                CheckForCaptures(piece);
            }

            // jesli mamy jakis ruch w ktorym zbijamy przeciwnika pozbedziemy
            // sie tych gdzie nie ma bicia
            if (allPossibleMoves.Any(move => move.PosToRemovePieces.Count > 0))
            {
                var newMovesList = allPossibleMoves.Where(move => move.PosToRemovePieces.Count > 0).ToList();
                allPossibleMoves = newMovesList;
            }
        }

        private void CheckForCaptures(Piece piece)
        {
            foreach (var direction in directions)
            {
                var startPos = piece.pos;
                var move = new Move(startPos);
                var enemyColor = ActiveColor == Piece.PieceColor.Black
                    ? Piece.PieceColor.White
                    : Piece.PieceColor.Black;
                var posToCheck = startPos + direction;
                if (PieceColorOnPos(posToCheck) == enemyColor)
                {
                    var posAfterCapture = posToCheck + direction;
                    var isNextFieldEmpty = CanMovePieceToPos(posAfterCapture);
                    if (isNextFieldEmpty)
                    {
                        move.AddNewStep(posAfterCapture);
                        allPossibleMoves.Add(move);
                    }
                }
            }
        }

        private Piece PieceOnPos(Vector2i pos)
        {
            foreach (var piece in pieces)
            {
                if (piece.Alive && piece.pos == pos)
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
                if (AnyFieldSelected && !IsAnyPieceOnPos(field.pos))
                {
                    var move = PossibleMoves.Count > 0 ? PossibleMoves.FirstOrDefault(move1 => move1.EndPos == field.pos) : null;
                    if (move != null)
                    {
                        MakeMove(move);
                    }
                }
                else if (PieceColorOnPos(field.pos) == ActiveColor)
                {
                    selectedField = field.pos;
                }
            }
        }

        void ChangePlayer()
        {
            AnyFieldSelected = false;
            ActiveColor = ActiveColor == Piece.PieceColor.White ? Piece.PieceColor.Black : Piece.PieceColor.White;
            UpdatePosibleMoves();
            if (IsGameEnded)
            {
                StartGame();
            }
        }

        Field FieldAtPos(Vector2i pos)
        {
            return fields.FirstOrDefault(field => field.pos == pos);
        }
    }
}