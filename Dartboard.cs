using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDartGame
{
    class Dartboard
    {
        // Klassens fält:
        private int[] dartboard = new int[] { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };


        /* Metoden frågar spelaren vilket nummer han siktar mot
           och returnerar detta nummer */
        public int DefineAimedNumber()
        {
            Console.WriteLine("Which number are you aiming for? ");
            int aimedNumber = 0;
            do
            {
                bool correctInput = false;
                do
                {
                    try         // för att programmet inte kraschar om inmatningen inte är heltal
                    {
                        aimedNumber = int.Parse(Console.ReadLine());
                        correctInput = true;
                    }
                    catch
                    {
                        Console.Write("Incorrect entry. Enter an integer between 1 and 20: ");
                    }
                } while (!correctInput);

                if (aimedNumber < 1 || aimedNumber > 20)        // Om användaren valt ett tal som inte finns på darttavlan
                {
                    Console.Write("Incorrect entry. Enter a number between 1 and 20: ");
                }
            } while (aimedNumber < 1 || aimedNumber > 20);      // Hela loopen körs tills användaren angett ett tal som finns på tavlan, dvs i vektorn

            return aimedNumber;
        }


        /* Metoden tar talet man siktar mot som argument 
           och returnerar dess index i vektorn dartboard */
        public int DefineAimedNumbersIndex(int aimedNumber)
        {
            int aimedNumbersIndex = Array.IndexOf(dartboard, aimedNumber);

            return aimedNumbersIndex;
        }


        /* Överlagrade metoden tar index av talet som man siktar mot som inparameter,
           och använder sannolikhet för att returnera poäng man får */
        public int ThrowDarts(int aimedNumbersIndex)
        {
            Random randomize = new Random();
            double percent = randomize.NextDouble();    // slumpa ett decimaltal från 0.0 till 1.0 som motsvarar sannolikhetsprocenter (0 % till 100 %)
            int score;

            if (percent < .05)          // 5 % sannolikt att missa tavlan => 0 poäng
            {
                score = 0;
            }
            else if (percent < .1)      // 5 % sannolikt att träffa en slumpmässig siffra (procentsatsen adderas till föregående procenten)
            {
                score = randomize.Next(1, 21);
            }
            else if (percent < .25)     // 15 % sannolikt att träffa poängen som är placerad efter den poäng man siktar mot; här behöver man talets index (+1)
            {
                if (aimedNumbersIndex == 19) // Förhindra att hamna ut ur vektorn om indexet är sist i vektorn
                {
                    score = dartboard[0];   // I så fall gå till vektorns början, index 0
                }
                else
                {
                    score = dartboard[aimedNumbersIndex + 1];    // annars öka indexet med 1
                }
            }
            else if (percent < .4)      // 15 % sannolikt att träffa poängen som är placerad innan den poäng man siktar mot
            {
                if (aimedNumbersIndex == 0)    // Förhindra att hamna ut ur vektorn om indexet är först i vektorn
                {
                    score = dartboard[19];    // I så fall gä till vektorns slut, indes 19
                }
                else
                {
                    score = dartboard[aimedNumbersIndex - 1];    // annars minska indexet med 1
                }
            }
            else        // Annars, kvarstående 60 % sannolikt att traffa den poäng man siktar mot
            {
                score = dartboard[aimedNumbersIndex];
            }

            Console.WriteLine("You scored {0}", score);

            return score;
        }


        /* Överlagrade metoden utan inparameter använder sannolikhet för att returnera poäng 
           så att man ha större chansen att helt missa tavlan
           samt att man ha större chansen att träffa tavlans övre hälften*/
        public int ThrowDarts()
        {
            Random randomize = new Random();
            double percent = randomize.NextDouble();    // slumpa ett decimaltal från 0.0 till 1.0 som motsvarar sannolikhetsprocenter (0 % till 100 %)
            int score;

            if (percent < .2)   // 20% sannolikt att missa tavlan, 0 poäng
            {
                score = 0;
            }
            else if (percent < .5) // 30 % sannolikt att träffa ett tal i tavlans övre hälften (index 0-4 och 15-19)
            {
                // för att kunna slumpa ett tal från två olika spann slumpa först ett tal mellan 1 och 2;
                // om talet är lika med 1, använda första spannet, annars använda andra spannet
                score = (randomize.Next(1, 3) == 1) ? randomize.Next(0, 5) : randomize.Next(15, 20);
            }
            else // Annars, kvarstående 50 % sannolikt att träffa ett tal i tavlans nedre helften (index 5-14)
            {
                score = randomize.Next(5, 15);
            }

            Console.WriteLine("You scored {0}", score);

            return score;
        }
    }
}
