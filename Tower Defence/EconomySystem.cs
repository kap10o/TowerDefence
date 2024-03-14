using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defence
{
    public class EconomySystem
    {
        private static int coins;
        private const int startingCoins = 180; // Starting coins for the player

        public int Coins { get { return coins; } }

        public EconomySystem()
        {
            coins = startingCoins;
        }

        // Method to deduct coins when a tower is built
        public bool DeductCoins(int amount)
        {
            if (coins >= amount)
            {
                coins -= amount;
                return true;
            }
            else
            {
                return false; // Not enough coins
            }
        }

        // Method to add coins when an enemy is defeated
        public static void AddCoins(int amount)
        {
            coins += amount;
        }
    }
}
