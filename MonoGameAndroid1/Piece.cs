namespace MonoGameAndroid1
{
    public class Piece
    {
        public enum PieceColor
        {
            White,
            Black
        }

        public bool isQueen = false;
        public Vector2i pos;
        public PieceColor color;

        public Piece(int x,int y, PieceColor color)
        {
            pos = new Vector2i(x,y);
            this.color = color;
        }

        public void MoveToPos(Vector2i newPos)
        {
            pos = newPos;
        }
    }
}