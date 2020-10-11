using System.Text;

namespace Arbitrage
{
    public static class ScraperFactory
    {
        static ScraperFactory()
        {
            buildMenu();
        }

        private static StringBuilder buildMenu()
        {
            StringBuilder newMenu = new StringBuilder();

            newMenu.Append(eTypeOfScrapers.Winner.ToString());

            return newMenu;
        }

        public enum eTypeOfScrapers
        {
            Winner
        }

        public static Scraper MakeNewScpraper(string i_Url)
        {
            StringBuilder menu = buildMenu();
            Scraper newScraper = null;
            string userChoice;

            userChoice = UI.printMenuToUserToGetNextAction(menu);

            if(userChoice == eTypeOfScrapers.Winner.ToString())
            {
                newScraper = new ScraperWinner(i_Url);
            }
            else
            {
                UI.PrintInvalidInput();
            }

            return newScraper;
        }
    }
}
