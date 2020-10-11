using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    static class UI
    {
        // create list/array of method to create new scraper (fro new kind of scraper)
        // client must create new class of new kind of scraper and new method to create to scraper
        private static Dictionary<char, char> m_hebWord = new Dictionary<char, char>();

        static UI()
        {
            Console.OutputEncoding = Encoding.GetEncoding("Windows-1255");
            makeDictOfHebWord();
        }

        public static string printMenuToUserToGetNextAction(StringBuilder i_Menu)
        {
            string inputFromUser;
            int numberTheUserChose;
            bool v_ValidInput = false;
            string[] arrOfMenu = i_Menu.ToString().Split('\n');
            string theActionTheUserChose = null;

            while (v_ValidInput == false)
            {
                int i = 0;

                foreach (string action in arrOfMenu)
                {
                    Console.WriteLine(string.Format(@"To {0} press {1}", action, i));
                    i++;
                }

                inputFromUser = Console.ReadLine();

                if (int.TryParse(inputFromUser, out numberTheUserChose))
                {
                    if (numberTheUserChose >= 0 && numberTheUserChose < i)
                    {
                        theActionTheUserChose = arrOfMenu[numberTheUserChose];
                        v_ValidInput = true;
                    }
                    else
                    {
                        Console.WriteLine("out of range try again");
                    }
                }
                else
                {
                    Console.WriteLine("invalid Input");
                }
            }

            return theActionTheUserChose;
        }

        private static void makeDictOfHebWord()
        {
            string hebChar = " .ףץםןאבגךדהוזחטיכלמנסעפצקרשת";

            foreach (char c in hebChar)
            {
                m_hebWord.Add(c, c);
            }

        }

        public static string ArrangeHebStringToBeHebUICustomize(string UnArrangeHebString)
        {
            StringBuilder tempString = new StringBuilder();
            Array arrayOfUnArrangeHebStringReversed = UnArrangeHebString.Reverse().ToArray();

            foreach (char c in arrayOfUnArrangeHebStringReversed)
            {
                if (m_hebWord.ContainsKey(c))
                {
                    tempString.Append(c);
                }
            }

            return tempString.ToString();
        }

        public static void PrintInvalidInput()
        {
            Console.WriteLine("Invalid Input");
        }

        public static void PrintString(string i_ToPrint)
        {
            Console.WriteLine(i_ToPrint);
        }

        public static string GetInputFromUser(string i_MesssageToUser)
        {
            bool v_ValidInput = false;
            string userInput = null;
            Console.WriteLine(i_MesssageToUser);

            while (v_ValidInput == false)
            {
                userInput = Console.ReadLine();
                v_ValidInput = userInput != string.Empty;
            }

            return userInput;
        }

        public static Scraper CreateNewScraperWithUser(Dictionary<string, Scraper> i_ExistedScrapers)
        {
            string urlOfScraper = UI.GetInputFromUser("Please Enter The URL of The New Scraper: ");
            Scraper newScraper = null;

            if (i_ExistedScrapers.ContainsKey(urlOfScraper) == false)
            {
                newScraper = ScraperFactory.MakeNewScpraper(urlOfScraper);

                if (newScraper == null)
                {
                    Console.WriteLine("The scraper you want to add wasn't been added");
                }
            }
            else
            {
                Console.WriteLine("Scraper Already Exist");
            }

            return newScraper;
        }
    }
}
