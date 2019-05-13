using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TheDartGame
{
    class Game
    {
        // Klassens fält:
        private List<Player> playerList = new List<Player>();

        string lines = new String('=', Console.WindowWidth);
        string indented = new String(' ', (Console.WindowWidth / 2) - 20);


        /* Metoden välkomnar spelare, skriver ut regler och
           anropar metoder för att bestämma antal spelare,
           registrera spelare och sätta igång spelet */
        public void StartNewGame()
        {
            // Välkomna spelare och skriva ut regler:
            Console.WriteLine("{0} \n {1}Welcome to the awesome 301 dart game! \n\n{0}", lines, indented);
            //Thread.Sleep(1000);

            Console.WriteLine("{0}Please read the game rules carefully: \n\n", indented);
            Thread.Sleep(1000);
            Console.WriteLine("   - Minimal number of players is two, maximal number is six.\n");
            Thread.Sleep(2000);
            Console.WriteLine("   - Each player starts with a score of 301 and takes turns to throw 3 darts.\n");
            Thread.Sleep(3000);
            Console.WriteLine("   - The score for each turn is calculated and deducted from the players total.\n");
            Thread.Sleep(3000);
            Console.WriteLine("   - The goal is to be the first player to reduce the score to exactly zero.\n");
            Thread.Sleep(3000);
            Console.WriteLine("   - If a player goes below zero, the score is bust, that turn ends immediately\n" +
                              "     and the score is returned to what it was at the start of that turn.\n");
            Thread.Sleep(6000);
            Console.WriteLine("   - When a player reaches zero, the other player(s) can decide whether or not to continue playing.\n\n");
            Thread.Sleep(6000);
            Console.WriteLine("     GOOD LUCK!");
            Thread.Sleep(1000);
            ClearAndProceed();
            // Regler hämtad från https://www.mastersofgames.com/rules/darts-rules.htm (med egna anpassningar)

            int numberOfPlayers = DefineNumberOfPlayers();    // Anropa metoden för att bestämma antal spelare och lagra metodens returvärde till variabeln
            RegisterPlayers(numberOfPlayers);                 // Anropa metoden för att registrera det bestämda antalet spelare
            ClearAndProceed();

            Console.WriteLine("{0} \n {1}Take your positions! \n\n{0}", lines, indented);
            Thread.Sleep(500);
            Console.WriteLine("POSITION\t NAME\n\n");
            for (int i = 0; i < playerList.Count; i++)    // Skriva ut spelarnas namn
            {
                Console.WriteLine("Player {0}:\t{1}", i + 1, playerList[i].GetName());
            }
            Thread.Sleep(500);
            string gameStarts = "The game starts NOW!";
            Console.WriteLine(String.Format("\n{0}\n\n{1," + ((Console.WindowWidth / 2) + (gameStarts.Length / 2)) + "}\n\n\n{0}", lines, gameStarts));
            ClearAndProceed();

            PlayGame();   // Anropa metoden för att sätta igång spelet

            ClearAndProceed();
        }


        /* Metoden ber användaren ange antal spelare (minst 2, högst 6),
           omvandlar användarens inmatning till heltal i try-catch
           och returnerar detta antal spelare */
        public int DefineNumberOfPlayers()
        {
            int numberOfPlayers = 0;
            Console.Write("Enter number of players: ");
            do
            {
                bool incorrectInput = true;
                do
                {
                    try
                    {
                        numberOfPlayers = int.Parse(Console.ReadLine());
                        incorrectInput = false;
                    }
                    catch
                    {
                        Console.Write("Incorrect input. Enter an integer: ");    // Förhindra att programmet kraschar om användarens inmatning inte är heltal
                    }
                } while (incorrectInput);    // do-while loopen köras sålänge inmatningen inte är heltal

                if (numberOfPlayers < 2)
                {
                    Console.Write("Minimal number of players is 2. Try again: ");
                }
                else if (numberOfPlayers > 6)
                {
                    Console.Write("Maximal number of players is 6. Try again: ");
                }
            } while (numberOfPlayers < 2 || numberOfPlayers > 6);    // Hela do-while (inkl. try-catch) köras sålänge inmatningen är mindre än två eller större än 6

            return numberOfPlayers;
        }


        /* Metoden ber användaren ange spelarnas namn
           eller trycka på x för att lägga till en dator-spelare
           och anroper metoden för att lägga spelare till listan */
        public void RegisterPlayers(int numberOfPlayers)
        {
            Console.WriteLine(lines + "\nEnter players' names (or press x to add a computer player instead)\n");
            string name;

            for (int i = 1; i <= numberOfPlayers; i++)    // for-loopen köras så många gånger som antal spelare
            {
                Console.Write("Player {0}: ", i);
                bool sameName;
                bool emptyName;
                bool whiteSpaceName;

                do
                {
                    do
                    {
                        name = Console.ReadLine();
                        emptyName = string.IsNullOrEmpty(name);             // Namn får inte vara en tom sträng
                        whiteSpaceName = string.IsNullOrWhiteSpace(name);   // eller en sträng som innehåller bara mellanslag
                        if (emptyName || whiteSpaceName)
                        {
                            Console.Write("Name cannot be empty. Try again: ");
                        }
                    } while (emptyName || whiteSpaceName);    // do-while loopen köras sålänge namn är tommt

                    sameName = false;
                    foreach (var player in playerList)        // Loopas igenom befintliga objekt i listan för att se till att namn inte upprepas
                    {
                        sameName = (player.GetName() == name) ? true : false;
                        if (sameName)
                        {
                            Console.Write("Name already in use. Choose a different name : ");
                            break;
                        }
                    }
                } while (sameName);    // Hela do-while loopen köras sålänge namn redan finns i listan

                if (name == "x" || name == "X")    // Om användaren tryckit x istället för namn
                {
                    AddPlayer();    // anropa metoden utan argument, för att använda defaulta värdet "BOT"
                }
                else
                {
                    AddPlayer(name);    // Annars skicka namnet som argument till metoden
                }
            }
        }


        /* Metoden skapar objekt av klassen Player och
           lägger dem till listan playerList av klassen Game */
        public void AddPlayer(string name = "BOT")    // Default name "BOT" används om metoden anropas utan name argument
        {
            Player newPlayer = new Player(name);    // Skapa ett objekt av klassen Player (skicka med spelarens namn, därför att Players konstruktor kräver namn)
            playerList.Add(newPlayer);              // Lägga objektet till listan 
        }


        /* Metoden som loopar igenom playerList, skriver spelarens namn 
           och anropar metoden som styr spelet */
        public void PlayGame()
        {
            bool keepPlaying = true;
            do
            {
                for (int i = 0; i < playerList.Count; i++)    // Loopas så många gånger som antal spelare i listan
                {                                             // Använder for istället för foreach, därför att for tillåter ändringar i listan
                    Console.WriteLine("{0}\n{1}{2}, it's your turn!", lines, indented, playerList[i].GetName());    // Skriva ut spelarens namn
                    int currentPoints = playerList[i].CalculatePoints();                               // Poäng beräknas efter varje kast så att spelaren har koll på denna för att t ex kunna sikta mot ett visst tal
                    Console.WriteLine("Your current points: {0}", currentPoints);                      // Skriva ut spelarens aktuella poäng

                    // keepPlaying är sannt tills:
                    // 1) en spelare vinner och de andra väljer att inte fortsätta spela
                    // 2) alla spelare har nått 301 poäng, dvs det finns inga spelare kvar i listan
                    keepPlaying = PlayersTurn(i, currentPoints);    // Metoden omfattar en omgång (dvs 3 kast) och returnera boolean om spelet ska fortsätta eller inte

                    if (!keepPlaying)  // Om spelare vill sluta spela
                    {
                        break;    // Avbryta for-loopet 
                    }
                    ClearAndProceed();
                }

                for (int i = 0; i < playerList.Count; i++)
                {
                    playerList[i].PrintTurns();
                }

                ClearAndProceed();

            } while (keepPlaying);

            Console.WriteLine("G A M E   O V E R");
        }


        /* Metoden tar spelarens index i listan och spelarens aktuella poäng som argument,
           omfattar en spelomgång (3 kast), drar uppnådda poäng av currentPoints
           och returnerar sant/falskt som styr om spelet ska fortsätta eller sluta */
        public bool PlayersTurn(int iPlayer, int currentPoints)
        {
            bool keepPlaying = true;
            int[] throwings = new int[3];    // Här lagras spelarens 3 kast som sedan läggs till turnsList

            for (int i = 0; i < throwings.Length; i++)    // För varje av 3 kast
            {
                if (currentPoints > 0)       // Ifall att spelaren uppnått 0 under omgången ska nästa kastet inte utföras
                {
                    throwings[i] = OneThrowing(i);    // Anropa metoden och lagra dess returvärde (= uppnådda poängen) i vektorn
                    currentPoints -= throwings[i];    // Aktuella poäng beräknas efter varje kast, för att förhindra nästa kastet om spelaren nått 0 poäng
                    Console.WriteLine("Your current points: {0}", currentPoints);    // Spelaren har alltid koll på sin aktuella poäng för att t ex kunna veta mot vilket tal han ska sikta

                    if (currentPoints == 0)    // Om spelaren nått 0 poäng
                    {
                        Console.WriteLine("\n\n{0} has reached zero! CONGRATULATIONS!", playerList[iPlayer].GetName());    // Spelaren har vunnit
                        playerList.Remove(playerList[iPlayer]);    // Ta bort vinnaren från listan
                        ClearAndProceed();

                        if (playerList.Count > 0)    // Om det finns minst en spelare kvar fråga om han/de vill fortsätta spela
                        {
                            for (int j = 0; j < playerList.Count; j++)
                            {
                                Console.Write(playerList[j].GetName() + ", ");
                            }
                            Console.WriteLine("do you want to continue playing?");
                            Console.WriteLine("- Select Y for YES");
                            Console.WriteLine("- Select N for NO");

                            string yesOrNo;
                            do
                            {
                                yesOrNo = Console.ReadLine().ToLower();  // Omvandla inmatningen till gemener för att göra svaret icke-skiftlägeskänsligt

                                if (yesOrNo != "y" && yesOrNo != "n")
                                {
                                    Console.Write("Invalid answer. Read the instruction and try again: ");
                                }
                            } while (yesOrNo != "y" && yesOrNo != "n");    // Frågan upprepas sålänge svaret skiljer sig från y eller n

                            if (yesOrNo == "y")     // Om spelare valt att fortsätta spela
                            {
                                keepPlaying = true;      // sant    
                                break;
                            }
                            else if (yesOrNo == "n")    // Om de valt att sluta spela
                            {
                                keepPlaying = false;    // blir falkst
                                break;                  // avbryta for-loop
                            }
                        }
                        break;  // avbryta for-loops
                    }
                }
            }
            if (currentPoints > 0)  // Om användaren inte nått 0 ännu
            {
                playerList[iPlayer].AddTurn(throwings[0], throwings[1], throwings[2]);      // Lägga till poäng genom AddTurn metoden av klassen Player
                ClearAndProceed();
                playerList[iPlayer].PrintTurns();
            }
            else if (currentPoints < 0)   // Om användaren hamnat under nollan nollställs poängen för den aktuella omgången
            {
                Console.WriteLine("\n\nYou went below zero. The score is bust and this turn ends.");
                Console.WriteLine("Score will be returned to what it was at the start of this turn.");

                playerList[iPlayer].AddTurn(0, 0, 0);   // Lägga till nollor istället av nådda poäng
                ClearAndProceed();
                playerList[iPlayer].PrintTurns();
            }

            return keepPlaying;
        }


        /* Metoden tar ett kasts index i vektorn som argument.
           Inför varje kast frågar metoden spelaren om han vill sikta mot ett tal eller kasta på måfå. 
           Metoden returnerar ett kasts poäng. */
        public int OneThrowing(int jThrowing)
        {
            int throwing = 0;
            Console.WriteLine("\n{0}Dart {1}:\n", indented, jThrowing + 1); // Skriva ut 1:a, 2:a eller 3:e kast

            Dartboard dartboard = new Dartboard();    // Skapa en instans av klassen Dartboard för att sedan kunna använda klassens metoder           

            switch (AimingOrRandom())   // Switcha metodens returvärde (1 eller 2)
            {
                case 1:                 // Spelaren valt att sikta mot ett tal
                    {
                        int aimedNumber = dartboard.DefineAimedNumber();                        // Metoden av klassen Dartboard som returnerar talet man siktar mot
                        int aimedNumbersIndex = dartboard.DefineAimedNumbersIndex(aimedNumber); // Skicka angivna talet som inparameter för att få talets index i vektorn
                        throwing = dartboard.ThrowDarts(aimedNumbersIndex);                     // Anropa metoden ThrowDarts och lagra dess returvärde till throwing; 
                        break;                                                                  // metoden är överlagrad, det är viktigt att skicka argument (aimedIndex)
                    }
                case 2:                 // Spelaren valt att kasta på måfå
                    {
                        throwing = dartboard.ThrowDarts();      // Anropa den överlagrade metoden utan argument♥
                        break;
                    }
            }

            return throwing;
        }


        /* Metoden frågar spelaren om han vill sikta mot ett visst tal eller kasta på måfå 
           och returneras spelarens svar (1 eller 2) */
        public int AimingOrRandom()
        {
            Console.WriteLine("Are you aiming for a particular number or are you throwing randomly?");
            Console.WriteLine("1 - particular number");
            Console.WriteLine("2 - random number");

            int option = 0;
            do
            {
                bool correctType = false;
                do
                {
                    try
                    {
                        option = int.Parse(Console.ReadLine());
                        correctType = true;
                    }
                    catch
                    {
                        Console.Write("Incorrect entry. Enter 1 or 2: ");    // Förhindra att programmet kraschar om svaret inte är heltal
                    }
                } while (!correctType); // Loopas sålänge svaret inte är heltal

                if (option != 1 && option != 2)
                {
                    Console.Write("Incorrect entry. Enter 1 or 2: ");
                }
            } while (option != 1 && option != 2);    // Loopas sålänge svaret skiljer sig från angivna alternativer

            return option;    // Metodens returvärde är 1 eller 2
        }


        /* Metoden som pausar utföringen tills användaren trycker något tangent för att fortsätta.
           Ingen viktig funktionalitet, bara "kosmetisk" */
        public void ClearAndProceed()
        {
            Console.WriteLine("\n\nPress any key to proceed . . .");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
