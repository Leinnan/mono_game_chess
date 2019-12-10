namespace MonoGameAndroid1
{
    public struct Vector2i
    {
        public int x;
        public int y;

        public Vector2i(int i, int i1)
        {
            this.x = i;
            this.y = i1;
        }
        
        public static bool operator ==(Vector2i c1, Vector2i c2) 
        {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Vector2i c1, Vector2i c2)
        {
            return !(c1 == c2);
        }
        
        public static Vector2i operator +(Vector2i c1, Vector2i c2) => new Vector2i(c1.x + c2.x, c1.y + c2.y);
        public static Vector2i operator /(Vector2i c1, int value) => new Vector2i(c1.x / value, c1.y / value);
    }
}