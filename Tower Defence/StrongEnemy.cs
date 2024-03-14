using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spline;

namespace Tower_Defence
{
    public class StrongEnemy : Enemy
    {
        // Additional properties specific to StrongEnemy
        private const int HitPoints = 6; // Initial hit points

        public StrongEnemy(Texture2D texture, SimplePath path, float initialPosition, float speed)
            : base(texture, path, initialPosition, HitPoints, speed)
        {
            // Additional initialization if needed
        }
    }
}
