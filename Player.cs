using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDartGame
{
    class Player
    {
        // Klassens fält
        private string name;
        private List<Turns> turnsList = new List<Turns>();

        // Konstruktor
        public Player(string _name)
        {
            name = _name;
        }


        /* Metoden får att komma åt objektens egenskap 'name' */
        public string GetName()
        {
            return name;
        }


        /* Metoden loopas igenom turnsList, drar av poäng från startpoäng
           och returnerar resultatet */
        public int CalculatePoints()
        {
            int points = 301;
            foreach (var turn in turnsList)                                     // Varje turn i Listan innehåller 3 "darts"
            {
                points -= turn.GetDart1() + turn.GetDart2() + turn.GetDart3();  // Värden av darts dras av points
            }

            return points;
        }


        /* Metoden tar 3 kast som inparameter,
           skapar objekt av klassen Turns
           och lägger dem till listan turnsList */
        public void AddTurn(int dart1, int dart2, int dart3)
        {
            turnsList.Add(new Turns(dart1, dart2, dart3));
        }


        /* Metoden som skriver ut spelarens statistik */
        public void PrintTurns()
        {
            int turnAll = 1;
            int total = CalculatePoints();

            Console.WriteLine("============================================\n" +
                              " {0}'s statistics:\n" +
                              "============================================\n" +
                              "                | Dart 1 | Dart 2 | Dart 3 |\n" +
                              "                ----------------------------", name);
            foreach (var turn in turnsList)
            {
                Console.WriteLine(" Turn  {0}\t|{1}", turnAll, turn);   // Skriva ut varje turn i spelarens turnList; använder Turns ToString()
                turnAll++;
            }
            Console.WriteLine("============================================");
            Console.WriteLine(" TOTAL: {0, 34} |", total);
            Console.WriteLine("============================================");
        }
    }
}
