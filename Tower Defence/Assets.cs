using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_Defence
{
    internal class Assets
    {
        internal static Texture2D backgroundTexture, texturebg, tdLogoTexture, infoTexture;
        internal static Texture2D slowtowerTexture, towerTexture;
        internal static Texture2D projectileTexture, projectileTextureblue;
        internal static Texture2D enemyTexture, strongEnemyTexture, heartTexture;
        internal static Texture2D looseTexure;
        internal static SpriteFont deffont;
        internal static void LoadAssets()
        {
            tdLogoTexture = Globals.Content.Load<Texture2D>("TDLogo");
            enemyTexture = Globals.Content.Load<Texture2D>("Enemy");
            strongEnemyTexture = Globals.Content.Load<Texture2D>("StrongEnemy");
            backgroundTexture = Globals.Content.Load<Texture2D>("TheMap");
            texturebg = Globals.Content.Load<Texture2D>("TheMapRender");
            towerTexture = Globals.Content.Load<Texture2D>("Tower1");
            slowtowerTexture = Globals.Content.Load<Texture2D>("Tower2");
            projectileTexture = Globals.Content.Load<Texture2D>("projectileTexture2");
            projectileTextureblue = Globals.Content.Load<Texture2D>("projectileTexture1");
            heartTexture = Globals.Content.Load<Texture2D>("heart");
            infoTexture = Globals.Content.Load<Texture2D>("infotab");
            looseTexure = Globals.Content.Load<Texture2D>("nowaydude");
            
            deffont = Globals.Content.Load<SpriteFont>("Deffont");
        }
    }
}
