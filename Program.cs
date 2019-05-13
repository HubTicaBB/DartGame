using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDartGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game newGame = new Game();    // Skapa en instans av klassen Game
            newGame.StartNewGame();       // Anropa metod av klassen Game som sätter igång spelet

            Console.ReadKey();
        }
    }
}
