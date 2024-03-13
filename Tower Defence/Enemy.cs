using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spline;


namespace Tower_Defence
{
    public class Enemy
    { 
        private Texture2D texture;
        private SimplePath path;
        private float texPos;

        public Enemy(Texture2D texture, SimplePath path, float initialPosition)
        {
            this.texture = texture;
            this.path = path;
            this.texPos = initialPosition;
        }

        public void Update(GameTime gameTime)
        {
            // Move the ball along the path
            texPos += 2; // Adjust speed as needed
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texPos != path.endT)
                spriteBatch.Draw(texture, path.GetPos(texPos), new Rectangle(0, 0, texture.Width, texture.Height),
                                 Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}

