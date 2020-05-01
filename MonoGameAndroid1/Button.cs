using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameAndroid1
{
    public class Button
    {
        private Texture2D bg;
        private SpriteFont font;
        private string text;
        private Rectangle rect;


        public bool IsPosOverRect(Vector2 pos)
        {
            return rect.Contains(pos);
        }

        public Button(Texture2D bg, SpriteFont font, string text, Rectangle rect)
        {
            this.bg = bg;
            this.font = font;
            this.text = text;
            this.rect = rect;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            var size = rect.Center.ToVector2() - (font.MeasureString(text) / 2);
            spriteBatch.Draw(bg, rect, Color.White);
            spriteBatch.DrawString(font, text, size, Color.White);
        }
    }
}