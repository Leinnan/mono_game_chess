namespace MonoGameAndroid1
{
    public class Piece
    {
        public enum PieceColor
        {
            White,
            Black,
            None
        }

        public bool isQueen = false;
        public Vector2i pos;
        public PieceColor color;
        public bool Alive;

        public bool MovesUp => isQueen || color == PieceColor.Black;
        public bool MovesDown => isQueen || color == PieceColor.White;

        public Piece(int x,int y, PieceColor color)
        {
            pos = new Vector2i(x,y);
            this.color = color;
            Alive = true;
        }

        public void MoveToPos(Vector2i newPos)
        {
            pos = newPos;
            if (color == PieceColor.White && pos.y == (Consts.BOARD_SIZE - 1) ||
                color == PieceColor.Black && pos.y == 0)
            {
                isQueen = true;
            }
        }

        public void Kill()
        {
            Alive = false;
        }
    }
}