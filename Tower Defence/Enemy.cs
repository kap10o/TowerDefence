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
    private Vector2 position;
    private Rectangle boundingBox; 
    public bool isAlive = true;
    public int hp;
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
        this.hp = initialHP;
        this.Speed = speed;
        this.heart = heart;
        
    }

    public virtual void Update(GameTime gameTime)
    {
        texPos += Speed;
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
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);

            Vector2 heartPosition = new Vector2(position.X -10, position.Y - 20);
            int heartsToDraw = hp;

            for (int i = 0; i < heartsToDraw; i++)
            {
                spriteBatch.Draw(Assets.heartTexture, heartPosition, Color.White);
                heartPosition.X += Assets.heartTexture.Width + 2;
            }
        }
    }

    public virtual void Hit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            isAlive = false;
            EconomySystem.AddCoins(10);
        }
    }
}