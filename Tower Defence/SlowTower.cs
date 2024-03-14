using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tower_Defence
{
    public class SlowTower : Tower
    {
        private List<Enemy> affectedEnemies;

        public SlowTower(Texture2D texture, Vector2 position, Texture2D projectileTexture)
            : base(texture, position, projectileTexture)
        {
            affectedEnemies = new List<Enemy>();
        }

        public override void Update(GameTime gameTime, List<Enemy> enemies)
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
                        if (!affectedEnemies.Contains(enemy) && Vector2.Distance(position, enemy.Position) < AttackRange)
                        {
                            // Apply slowing effect if it hasn't been applied yet
                            enemy.Speed -= 0.5f; // Reduce enemy speed by half
                            affectedEnemies.Add(enemy); // Add enemy to list of affected enemies
                        }
                        // Fire a projectile at the enemy
                        FireProjectile(enemy);
                        timeSinceLastShot = 0; // Reset shot timer
                        // Apply slowing effect if it hasn't been applied yet
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
