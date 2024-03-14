using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spline;
using Tower_Defence;
using System;

public class Enemy
{
    private Texture2D texture;
    private Texture2D heart;
    private SimplePath path;
    public float texPos;
    private Vector2 position; // Position of the enemy
    private Rectangle boundingBox; // Bounding box for collision detection
    public bool isAlive = true; // Flag indicating whether the enemy is alive
    public int hp; // Hit points of the enemy
    public float Speed { get; set; }
    public Vector2 Position { get { return position; } }
    public Rectangle BoundingBox { get { return boundingBox; } }
    public bool IsAlive { get { return isAlive; } }

    public Enemy(Texture2D texture, SimplePath path, float initialPosition, int initialHP, float speed, Texture2D heart)
    {
        this.texture = texture;
        this.path = path;
        this.texPos = initialPosition;
        this.position = path.GetPos(initialPosition);
        this.boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        this.hp = initialHP; // Initialize HP
        this.Speed = speed;
        this.heart = heart;
        
    }

    public virtual void Update(GameTime gameTime)
    {
        texPos += Speed; // Adjust speed as needed
        if (isAlive)
        {
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
            // Calculate the position for drawing the heart
            Vector2 heartPosition = new Vector2(position.X -10, position.Y - 20); // Adjust Y position for heart above the enemy

            // Calculate the number of hearts to draw based on the remaining health
            int heartsToDraw = hp;

            // Draw hearts based on the number of hearts to draw
            for (int i = 0; i < heartsToDraw; i++)
            {
                spriteBatch.Draw(heart, heartPosition, Color.White);
                heartPosition.X += heart.Width + 2; // Adjust X position for the next heart
            }
        }
    }

    // Method to check if the enemy is hit by a projectile
    public virtual void Hit(int damage)
    {
        hp -= damage; // Reduce HP by damage
        if (hp <= 0)
        {
            isAlive = false; // If HP drops to or below 0, mark the enemy as dead
            EconomySystem.AddCoins(10);
        }
    }
}