using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_Defence
{
    internal class Assets
    {
        public static Texture2D backgroundTexture, enemyTexture, towerTexture;

        public static void LoadTextures()
        {
            backgroundTexture = Globals.Content.Load<Texture2D>("TheMap");
            enemyTexture = Globals.Content.Load<Texture2D>("Enemy");
            towerTexture = Globals.Content.Load<Texture2D>("Tower");
        }
    }
}
