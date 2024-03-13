using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spline;

public class Enemy
{
    private Texture2D texture;
    private SimplePath path;
    private float texPos;
    private Vector2 position; // Position of the enemy
    private Rectangle boundingBox; // Bounding box for collision detection
    private bool isAlive = true; // Flag indicating whether the enemy is alive

    public Vector2 Position { get { return position; } }

    public Rectangle BoundingBox { get { return boundingBox; } }

    public bool IsAlive { get { return isAlive; } }

    public Enemy(Texture2D texture, SimplePath path, float initialPosition)
    {
        this.texture = texture;
        this.path = path;
        this.texPos = initialPosition;
        this.position = path.GetPos(initialPosition);
        this.boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
    }

    public void Update(GameTime gameTime)
    {
        if (isAlive)
        {
            texPos += 1; // Adjust speed as needed
            position = path.GetPos(texPos);
            boundingBox.Location = position.ToPoint();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (isAlive)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height),
                             Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }

    // Method to check if the enemy is hit by a projectile
    public void Hit()
    {
        isAlive = false;
    }
}