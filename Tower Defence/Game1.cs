using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using Spline;
using System;
using System.Collections.Generic;

namespace Tower_Defence
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D enemyTexture;
        Texture2D backgroundTexture;
        SimplePath path;

        private Texture2D texturebg;
        private RenderTarget2D _renderTarget;
        List<GameObject> placedObjects = new List<GameObject>();
        GameObject currentObject;

        private readonly int width = 1200;
        private readonly int height = 800;

        private List<Enemy> enemies = new List<Enemy>(); // List to hold ball objects
        private float nextEnemyReleaseTime = 0; // Time when next ball should be released
        private float enemyReleaseInterval = 1000; // Interval between releasing balls (in milliseconds)

        public float texPos;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = height;
            _graphics.PreferredBackBufferWidth = width;
            _graphics.ApplyChanges();
            _renderTarget = new RenderTarget2D(GraphicsDevice,
            Window.ClientBounds.Width, Window.ClientBounds.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            path = new SimplePath(_graphics.GraphicsDevice);
            //path.generateDefaultPath(); //behövs inte, finns redan en från början

            path.Clean(); // tar bort alla punkter

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            enemyTexture = Content.Load<Texture2D>("Enemy");
            backgroundTexture = Content.Load<Texture2D>("TheMap");

            //sätter bildens startpunkt till början av kurvan
            texPos = path.beginT;
            //path.SetPos(0, Vector2.Zero);
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
            //path.GetPos(path.beginT);
            
            currentObject = new GameObject
            {
                texture = Content.Load<Texture2D>("Tower"), // Replace with your actual file name
                position = Vector2.Zero // You can set the initial position to (0, 0) or any other starting position
            };

            // Initialize hitbox after setting the texture
            currentObject.hitbox = new Rectangle(0, 0, currentObject.texture.Width, currentObject.texture.Height);
            texturebg = Content.Load<Texture2D>("TheMapRender");
            DrawOnRenderTarget();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // Update timer and release balls if necessary
            nextEnemyReleaseTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (nextEnemyReleaseTime <= 0)
            {
                enemies.Add(new Enemy(enemyTexture, path, path.beginT)); // Release a new ball
                nextEnemyReleaseTime = enemyReleaseInterval; // Reset timer
            }

            // Update all balls
            foreach (var Enemy in enemies)
            {
                Enemy.Update(gameTime);
            }


            //förflyttar positionen längs kurvan
            texPos++; //bestämmer hastigheten
            //texPos++;
            currentObject.position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            // Update hitbox for the currentObject
            currentObject.hitbox = new Rectangle((int)currentObject.position.X, (int)currentObject.position.Y,
                currentObject.texture.Width, currentObject.texture.Height);

            // Check for mouse click to place the object
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                // Check if the object can be placed
                if (CanPlace(currentObject))
                {
                    // Add the object to the list of placed objects
                    placedObjects.Add(new GameObject
                    {
                        texture = currentObject.texture,
                        position = currentObject.position,
                        hitbox = currentObject.hitbox
                    });
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // Draw the render target
            _spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);

            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, width, height), Color.White);
            //ritar ut kurvan
            //path.Draw(_spriteBatch);
            //ritar ut punkterna på kurvan
            //path.DrawPoints(_spriteBatch);
            //ritar ut trollkarlen på kurvan
            foreach (var Enemy in enemies)
            {
                Enemy.Draw(_spriteBatch);
            }
            // Draw all placed objects
            foreach (var obj in placedObjects)
            {
                _spriteBatch.Draw(obj.texture, obj.position, Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);

        }

        private void DrawOnRenderTarget()
        {
            //Ändra så att GraphicsDevice ritar mot vårt render target
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin();

            //Rita ut texturen. Den ritas nu ut till vårt render target istället
            //för på skärmen.
            _spriteBatch.Draw(texturebg, Vector2.Zero, Color.White);
            _spriteBatch.End();

            //Sätt GraphicsDevice att åter igen peka på skärmen
            GraphicsDevice.SetRenderTarget(null);
        }
        public bool CanPlace(GameObject g)
        {
            Color[] pixels = new Color[g.texture.Width * g.texture.Height];
            Color[] pixels2 = new Color[g.texture.Width * g.texture.Height];
            g.texture.GetData<Color>(pixels2);
            _renderTarget.GetData(0, g.hitbox, pixels, 0, pixels.Length);
            for (int i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i].A > 0.0f && pixels2[i].A > 0.0f)
                    return false;
            }
            // So that you can't place any objects where there all ready are ones.
            foreach (var obj in placedObjects)
            {
                if (g.hitbox.Intersects(obj.hitbox))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
