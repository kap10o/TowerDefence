using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public static class Globals
    {
        public static float Time { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static Point WindowSize { get; set; }

        public static int width = 1200;
        public static int height = 800;

        public static void Update(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
