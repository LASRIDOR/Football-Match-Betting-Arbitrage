using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    public class JobExecuter : IJob
    {

        private void writeTimeAndDateToDebug()
        {
            var message = $"JobExecuter executed at ${DateTime.Now.ToString()}";
            System.Diagnostics.Debug.WriteLine(message);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            writeTimeAndDateToDebug();

            Dictionary<string, Scraper> Scrapers = (Dictionary<string, Scraper>)context.JobDetail.JobDataMap.Get("DictOfScraper");
            List<FootballMatch> footballMatchesToBetOn = new List<FootballMatch>();

            foreach (Scraper scraper in Scrapers.Values)
            {
                try
                {
                    scraper.LoadUrl();
                    List<FootballMatch> FootballMatchesFromScraper = scraper.MakeListOfDailyMatchesPlaying();

                    foreach (FootballMatch match in FootballMatchesFromScraper)
                    {
                        FootballMatch tempMatch = match;

                        if (Arbitrager.isArbitrage(ref tempMatch) == true)
                        {
                            Arbitrager.GamblingRatio(ref tempMatch);
                            tempMatch.MatchStats = scraper.StatsCollector(tempMatch.StatsUrl);
                            footballMatchesToBetOn.Add(tempMatch);
                        }
                    }

                    foreach (FootballMatch match in footballMatchesToBetOn)
                    {
                        Console.WriteLine(UI.ArrangeHebStringToBeHebUICustomize(" מהכסף שלך.")+ match.FirstTeamGamble + " " + match.FirstTeam + UI.ArrangeHebStringToBeHebUICustomize(" מהכסף שלך, ועל הקבוצה ")+ match.SecondTeamGamble + " " + match.SecondTeam + UI.ArrangeHebStringToBeHebUICustomize("תהמר על הקבוצה "));
                        Console.WriteLine(match.MatchStats);
                    }
                    // bet/send message/dont know on arbitrage game (footballMatchesToBetOn)
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
