using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDartGame
{
    class Turns
    {
        // Klassens fält:
        private int dart1;
        private int dart2;
        private int dart3;

        // Konstruktor:
        public Turns(int _dart1, int _dart2, int _dart3)
        {
            dart1 = _dart1;
            dart2 = _dart2;
            dart3 = _dart3;
        }


        /* 3 Metoder för att komma åt 3 darts */
        public int GetDart1()
        {
            return dart1;
        }

        public int GetDart2()
        {
            return dart2;
        }

        public int GetDart3()
        {
            return dart3;
        }


        public override string ToString()
        {
            return string.Format("{0, 7} |{1, 7} |{2, 7} |", dart1, dart2, dart3);
        }
    }
}
