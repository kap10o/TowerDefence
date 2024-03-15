using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spline;

namespace Tower_Defence
{
    public class StrongEnemy : Enemy
    {
        private const int HitPoints = 5; 
        private const int sSpeed = 2;

        public StrongEnemy(Texture2D texture, SimplePath path, float initialPosition, float speed, Texture2D heart)
            : base(texture, path, initialPosition, HitPoints, sSpeed, heart)
        {
            
        }
    }
}
