using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Tower_Defence
{
    public class Projectile
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private float speed;
        private int damage;
        private Rectangle boundingBox;
        private Enemy target;

        public bool IsActive { get; private set; }

        public Projectile(Texture2D texture, Vector2 position, Vector2 direction, float speed, int damage)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = direction * speed;
            this.speed = speed * 30;
            this.damage = damage;
            IsActive = true;

            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            if (target == null || !target.IsAlive)
            {
                FindNearestEnemy(enemies);
            }

            if (target != null)
            {
                Vector2 direction = Vector2.Normalize(target.Position - position);
                velocity = direction;
                position += velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                boundingBox.Location = position.ToPoint();

                if (boundingBox.Intersects(target.BoundingBox))
                {
                    target.Hit(damage);
                    IsActive = false;
                }
            }
            else
            {
                IsActive = false;
            }
        }

        private void FindNearestEnemy(List<Enemy> enemies)
        {
            float shortestDistance = float.MaxValue;
            Enemy nearestEnemy = null;

            foreach (Enemy enemy in enemies)
            {
                if (enemy.IsAlive)
                {
                    float distance = Vector2.DistanceSquared(position, enemy.Position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }

            target = nearestEnemy;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = position - new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, drawPosition, Color.White);
        }
    }
}
