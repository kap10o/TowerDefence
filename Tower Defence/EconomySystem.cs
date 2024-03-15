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
        private const int startingCoins = 120;

        public static int Coins { get { return coins; } }

        public EconomySystem()
        {
            coins = startingCoins;
        }

        public bool DeductCoins(int amount)
        {
            if (coins >= amount)
            {
                coins -= amount;
                return true;
            }
            else
            {
                return false; 
            }
        }

        public static void AddCoins(int amount)
        {
            coins += amount;
        }
    }
}
