using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spline;
using System;
using System.Collections.Generic;
using WinForm;

namespace Tower_Defence
{
    public enum GameState { Start, Game, GameOver}
    public class Game1 : Game
    {
        public GameState previousGameState;
        public GameState currentGameState = GameState.Start;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private EconomySystem economy;
        ParticleSystem particleSystem;
        
        SimplePath path;
        Form1 myForm;
        
        private RenderTarget2D _renderTarget;
        private Texture2D texturebg;

        private readonly int width = 1200;
        private readonly int height = 800;

        private List<Enemy> enemies = new List<Enemy>(); 
        private float nextEnemyReleaseTime = 0; 
        private float enemyReleaseInterval = 1000;
        private bool spawnRegularEnemy = true;
        private float enemySpeed = 1.0f;
        private SpriteFont deffont;
        private List<Tower> towers = new List<Tower>();
        Tower currentTower;
        public float texPos;

        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            economy = new EconomySystem();
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = height;
            _graphics.PreferredBackBufferWidth = width;
            _graphics.ApplyChanges();
            _renderTarget = new RenderTarget2D(GraphicsDevice,
            Window.ClientBounds.Width, Window.ClientBounds.Height);
            Globals.Content = Content;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            path = new SimplePath(_graphics.GraphicsDevice);

            Assets.LoadAssets();
            
            path.Clean();

            texPos = path.beginT;
            
            path.AddPoint(new Vector2(994, 0));
            path.AddPoint(new Vector2(950, 205));
            path.AddPoint(new Vector2(870, 200));
            path.AddPoint(new Vector2(785, 145));
            path.AddPoint(new Vector2(670, 105));
            path.AddPoint(new Vector2(355, 200));
            path.AddPoint(new Vector2(265, 390));
            path.AddPoint(new Vector2(350, 610));
            path.AddPoint(new Vector2(700, 705));
            path.AddPoint(new Vector2(795, 670));
            path.AddPoint(new Vector2(865, 605));
            path.AddPoint(new Vector2(815, 540));
            path.AddPoint(new Vector2(700, 605));
            path.AddPoint(new Vector2(575, 620));
            path.AddPoint(new Vector2(370, 410));
            path.AddPoint(new Vector2(590, 200));
            path.AddPoint(new Vector2(670, 235));
            path.AddPoint(new Vector2(630, 290));
            path.AddPoint(new Vector2(565, 300));
            path.AddPoint(new Vector2(495, 365));
            path.AddPoint(new Vector2(530, 490));
            path.AddPoint(new Vector2(675, 510));
            path.AddPoint(new Vector2(760, 415));
            path.AddPoint(new Vector2(900, 413));
            path.AddPoint(new Vector2(1050, 413));
            path.AddPoint(new Vector2(1200, 410));

            currentTower = new Tower(
            Assets.towerTexture, 
            position: Vector2.Zero, 
            projectileTexture: Assets.projectileTexture
            );

            currentTower.hitbox = new Rectangle(0, 0, currentTower.texture.Width, currentTower.texture.Height);
            texturebg = Assets.texturebg;
            DrawOnRenderTarget();

            List<Texture2D> textures = new List<Texture2D>
            {
                Assets.enemyTexture
            };
            particleSystem = new ParticleSystem(textures, new Vector2(400, 240));

            myForm = new Form1();
            myForm.Show();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentGameState)
            {
                case GameState.Start:
                    {
                        particleSystem.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                        particleSystem.Update();
                        if (myForm.changeState == true)

                            currentGameState = GameState.Game;
                    }
                    break;
                case GameState.Game:
                    {
                        nextEnemyReleaseTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (nextEnemyReleaseTime <= 0)
                        {
                            if (spawnRegularEnemy)
                            {
                                enemies.Add(new Enemy(Assets.enemyTexture, path, path.beginT, 3, enemySpeed, Assets.heartTexture));
                            }
                            else
                            {
                                enemies.Add(new StrongEnemy(Assets.strongEnemyTexture, path, path.beginT, enemySpeed, Assets.heartTexture));
                            }

                            spawnRegularEnemy = !spawnRegularEnemy;

                            nextEnemyReleaseTime = enemyReleaseInterval;
                        }

                        foreach (var enemy in enemies)
                        {
                            enemy.Update(gameTime);
                        }
                        
                        texPos++; 

                        currentTower.position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                        
                        currentTower.hitbox = new Rectangle((int)currentTower.position.X, (int)currentTower.position.Y, currentTower.texture.Width, currentTower.texture.Height);

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            Tower newTower = new Tower(Assets.towerTexture, Mouse.GetState().Position.ToVector2(), Assets.projectileTexture);

                            if (CanPlace(newTower) && economy.DeductCoins(40))
                            {
                                towers.Add(newTower);
                            }
                        }

                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            Tower newTower = new SlowTower(Assets.slowtowerTexture, Mouse.GetState().Position.ToVector2(), Assets.projectileTextureblue);

                            if (CanPlace(newTower) && economy.DeductCoins(60))
                            {
                                towers.Add(newTower);
                            }
                        }

                        foreach (var tower in towers)
                        {
                            tower.Update(gameTime, enemies);
                        }
                        break;
                    }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.Start:
                    {
                        GraphicsDevice.Clear(Color.DarkGreen);
                        _spriteBatch.Begin();
                        _spriteBatch.Draw(Assets.tdLogoTexture, Vector2.Zero, Color.White);
                        _spriteBatch.End();
                        particleSystem.Draw(_spriteBatch);
                    }
                    break;
                case GameState.Game:
                    {
                        GraphicsDevice.Clear(Color.CornflowerBlue);
                        _spriteBatch.Begin();

                        _spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);

                        _spriteBatch.Draw(Assets.backgroundTexture, new Rectangle(0, 0, width, height), Color.White);

                        //path.Draw(_spriteBatch);
                        //path.DrawPoints(_spriteBatch);

                        foreach (var Enemy in enemies)
                        {
                            Enemy.Draw(_spriteBatch);
                        }

                        foreach (var tower in towers)
                        {
                            _spriteBatch.Draw(tower.texture, tower.position, Color.White);

                            foreach (var projectile in tower.projectiles)
                            {
                                projectile.Draw(_spriteBatch);
                            }
                        }
                        _spriteBatch.Draw(Assets.infoTexture, Vector2.Zero, Color.White);
                        _spriteBatch.DrawString(Assets.deffont, EconomySystem.Coins.ToString(), new Vector2(60, 20), Color.Black);
                        _spriteBatch.End();
                    }
                    break;
            }
            base.Draw(gameTime);

        }

        private void DrawOnRenderTarget()
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(Assets.texturebg, Vector2.Zero, Color.White);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }
        public bool CanPlace(Tower t)
        {
            Color[] pixels = new Color[t.texture.Width * t.texture.Height];
            Color[] pixels2 = new Color[t.texture.Width * t.texture.Height];
            t.texture.GetData<Color>(pixels2);
            _renderTarget.GetData(0, t.hitbox, pixels, 0, pixels.Length);
            for (int i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i].A > 0.0f && pixels2[i].A > 0.0f)
                    return false;
            }

            foreach (var tower in towers)
            {
                if (t.hitbox.Intersects(tower.hitbox))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
