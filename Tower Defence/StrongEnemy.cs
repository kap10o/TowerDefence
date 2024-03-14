﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spline;

namespace Tower_Defence
{
    public class StrongEnemy : Enemy
    {
        // Additional properties specific to StrongEnemy
        private const int HitPoints = 5; // Initial hit points
        private const int sSpeed = 2;

        public StrongEnemy(Texture2D texture, SimplePath path, float initialPosition, float speed, Texture2D heart)
            : base(texture, path, initialPosition, HitPoints, sSpeed, heart)
        {
            // Additional initialization if needed
        }
    }
}
