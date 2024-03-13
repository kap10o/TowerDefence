using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tower_Defence
{
    public class GameObject
    {
        public Texture2D texture;  // The texture of the object
        public Vector2 position;   // The position of the object
        public Rectangle hitbox;   // The hitbox of the object
        public float AttackRange { get; set; } // The attack range of the object

        // Additional properties or methods can be added as needed
    }
}
