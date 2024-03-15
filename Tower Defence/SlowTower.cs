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

            if (timeSinceLastShot >= fireCooldown)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (Vector2.Distance(position, enemy.Position) < AttackRange)
                    {
                        if (!affectedEnemies.Contains(enemy) && Vector2.Distance(position, enemy.Position) < AttackRange)
                        {
                            enemy.Speed -= 0.5f; 
                            affectedEnemies.Add(enemy);
                        }

                        FireProjectile(enemy);
                        timeSinceLastShot = 0;
                        break;
                    }
                }
            }

            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime, enemies);
            }

            projectiles.RemoveAll(p => !p.IsActive);
        }

        private void FireProjectile(Enemy target)
        {
            Vector2 initialPosition = position + new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 direction = Vector2.Normalize(target.Position - initialPosition);

            Projectile projectile = new Projectile(projectileTexture, initialPosition, direction, 10.0f, 1);
            projectiles.Add(projectile);
        }
    }
}
