namespace MonoGameAndroid1
{
    public class Field
    {
        public Vector2i pos;
        public bool isDark = false;

        public Field(int x, int y)
        {
            isDark = (x + y) % 2 == 0;
            pos = new Vector2i(x,y);
        }
    }
}