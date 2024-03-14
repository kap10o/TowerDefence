using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Tower_Defence
{
    public class Tower
    {
        public Texture2D texture;
        public Texture2D projectileTexture;
        public Vector2 position;
        public Rectangle hitbox;
        public List<Projectile> projectiles;
        internal float fireCooldown = 1.0f; // Cooldown in seconds between each shot
        public float AttackRange = 100f;
        internal float timeSinceLastShot = 0;

        public Tower(Texture2D texture,  Vector2 position, Texture2D projectileTexture)
        {
            this.texture = texture;
            this.position = position;
            this.projectiles = new List<Projectile>();
            this.projectileTexture = projectileTexture;
            this.hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public virtual void Update(GameTime gameTime, List<Enemy> enemies)
        {
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check if enough time has passed since last shot
            if (timeSinceLastShot >= fireCooldown)
            {
                foreach (Enemy enemy in enemies)
                {
                    // Check if the enemy is within range of the tower
                    if (Vector2.Distance(position, enemy.Position) < AttackRange)
                    {
                        // Fire a projectile at the enemy
                        FireProjectile(enemy);
                        timeSinceLastShot = 0; // Reset shot timer
                        break; // Only fire at one enemy per update
                    }
                }
            }
        
            

            // Update projectiles
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime, enemies);
            }

            // Remove inactive projectiles
            projectiles.RemoveAll(p => !p.IsActive);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

            // Draw projectiles
            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }

        private void FireProjectile(Enemy target)
        {
            // Calculate the initial position of the projectile based on the tower's position and the target enemy's position
            Vector2 initialPosition = position + new Vector2(texture.Width / 2, texture.Height / 2); // Center of the tower
            Vector2 direction = Vector2.Normalize(target.Position - initialPosition);

            // Create the projectile at the initial position
            Projectile projectile = new Projectile(projectileTexture, initialPosition, direction, 10.0f, 1);
            projectiles.Add(projectile);
        }
    }
}
