using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;

namespace Arbitrage
{
    public class SystemManager
    {
        public enum eMenu {
            Quit,
            AddScraperToDict,
            RemoveScraperFromDict,
            PrintAllScrapers,
            PrintAllNotConnectedUrl,
            StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage
        }

        private IScheduler m_schedueler;
        private readonly List<string> m_NotConnectedUrl = new List<string>();
        private readonly Dictionary<string, Scraper> r_Scrapers = new Dictionary<string, Scraper>(); // key = url, value = scraper
        private static bool v_StartedSceduleForScraper = false;

        public SystemManager()
        {
            m_schedueler = this.SchedulerConfig();
        }

        public void OpenSystemManagerForArbitrageFounder()
        {
            string userChoise;
            StringBuilder menu = buildMainMenuFromeMemu();

            userChoise = earaseSpaceInString(UI.printMenuToUserToGetNextAction(menu));

            while (userChoise != eMenu.Quit.ToString())
            {
                if (userChoise == eMenu.AddScraperToDict.ToString())
                {
                    getScraperFromUserAndAdd();
                }
                else if (userChoise == eMenu.RemoveScraperFromDict.ToString())
                {
                    getUrlToOfScraperToRemove();
                }
                else if (userChoise == eMenu.PrintAllScrapers.ToString())
                {
                    printAllScapers();
                }
                else if (userChoise == eMenu.PrintAllNotConnectedUrl.ToString())
                {
                    printNotConnectedScapers();
                }
                else if (userChoise == eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage.ToString())
                {

                    if (v_StartedSceduleForScraper == false)
                    {
                        this.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage();
                        menu.Remove(menu.Length - 1 - seperateLineBySpaceBeforeCapitalLetter(eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage.ToString()).Length
                            , seperateLineBySpaceBeforeCapitalLetter(eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage.ToString()).Length + 1);
                    }
                    else
                    {
                        UI.PrintInvalidInput();
                    }
                }
                else
                {
                    UI.PrintInvalidInput();
                }

                userChoise = earaseSpaceInString(UI.printMenuToUserToGetNextAction(menu));
            }
        }

        private void getScraperFromUserAndAdd()
        {
            Scraper userScraper = UI.CreateNewScraperWithUser(r_Scrapers);

            if (userScraper != null)
            {
                this.AddScraperToDict(userScraper);
            }
        }

        private bool AddScraperToDict(Scraper i_NewScraper)
        {
            bool v_ScraperAdded = false;

            //add event
            i_NewScraper.AddActionDelegate(this.OnFailConnection);
            // add to dict
            r_Scrapers.Add(i_NewScraper.WebsiteUrl, i_NewScraper);

            return v_ScraperAdded;
        }

        private void getUrlToOfScraperToRemove()
        {
            System.Text.StringBuilder menuOfUrlToRempove = new System.Text.StringBuilder();

            buildMenuOfUrlToRemove(menuOfUrlToRempove);

            string urlToRemove = UI.printMenuToUserToGetNextAction(menuOfUrlToRempove);
            bool v_ScraperRemoved = this.RemoveScraperFromDict(urlToRemove);

            if (v_ScraperRemoved == false)
            {
                UI.PrintInvalidInput();
            }
        }

        private bool RemoveScraperFromDict(string i_Url)
        {
            bool v_ScraperBeenRemoved = false;

            // find i_Url
            if (r_Scrapers.ContainsKey(i_Url) == true)
            {
                v_ScraperBeenRemoved = true;
                r_Scrapers.Remove(i_Url);
            }
            else
            {
                v_ScraperBeenRemoved = false;
                Console.WriteLine("couldnt find " + i_Url);
            }
            // remove from dictionary
            return v_ScraperBeenRemoved;
        }

        private void buildMenuOfUrlToRemove(StringBuilder menuOfUrlToRempove)
        {
            foreach (string url in this.r_Scrapers.Keys)
            {
                menuOfUrlToRempove.Append(url);
            }
        }

        private void printAllScapers()
        {
            System.Text.StringBuilder urlOfScrapers = new System.Text.StringBuilder();

            foreach (string url in r_Scrapers.Keys)
            {
                urlOfScrapers.AppendLine(url);
            }

            UI.PrintString(urlOfScrapers.ToString());
        }

        private void printNotConnectedScapers()
        {
            System.Text.StringBuilder urlOfNotConnectedScrapers = new System.Text.StringBuilder();

            foreach (string url in this.m_NotConnectedUrl)
            {
                urlOfNotConnectedScrapers.AppendLine(url);
            }

            UI.PrintString(urlOfNotConnectedScrapers.ToString());
        }

        // in video youtube return Task<IActionResult> (https://www.youtube.com/watch?v=4HPY3Mk5TsA&list=PLSi1BNmQ61ZohCcl43UdAksg7X3_TSmly&index=9)
        private async void StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage() {
            v_StartedSceduleForScraper = true;

            IJobDetail jobDetail = JobBuilder.Create<JobExecuter>()
                .WithIdentity("Arbitrager", "DailyGetAllFootballMatchFromScrapersAndCalculateArbitrage")
                .Build();

            jobDetail.JobDataMap.Put("DictOfScraper", this.r_Scrapers);

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("Arbitrager", "DailyBasis")
                .StartNow()
                .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(00, 20)).EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 59)).WithIntervalInMinutes(1))
                .Build();

            await m_schedueler.ScheduleJob(jobDetail, trigger);

            //return RedirectToAction("index");
        }

        private Quartz.IScheduler SchedulerConfig()
        {
            NameValueCollection props = new NameValueCollection {
            { "quartz.serializer.type","binary"}
        };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            var scheduler = factory.GetScheduler().Result;

            scheduler.Start().Wait();

            return scheduler;
        }

        private void OnShutdown()
        {
            if (m_schedueler.IsShutdown == false)
            {
                m_schedueler.Shutdown();
            }
        }

        private void OnFailConnection(object sender, string i_Url)
        {
            Console.WriteLine(i_Url + " Scraper couldn't connect and have been removed."+ Environment.NewLine +
                "you can find the unable to connect url on NotConnectedUrl List");
            m_NotConnectedUrl.Add(i_Url);
            this.RemoveScraperFromDict(i_Url);
        }

        private StringBuilder buildMainMenuFromeMemu()
        {
            StringBuilder newMenu = new StringBuilder();

            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.Quit.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.AddScraperToDict.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.RemoveScraperFromDict.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.PrintAllScrapers.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.PrintAllNotConnectedUrl.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage.ToString()));

            return newMenu;
        }

        private static string seperateLineBySpaceBeforeCapitalLetter(string i_Line)
        {
            StringBuilder newSeperatedLine = new StringBuilder();

            foreach (char c in i_Line)
            {
                if (c >= 'A' && c <= 'Z' && newSeperatedLine.Length != 0)
                {
                    newSeperatedLine.Append(' ');
                }

                newSeperatedLine.Append(c);
            }

            return newSeperatedLine.ToString();
        }

        private static string earaseSpaceInString(string i_Line)
        {
            StringBuilder newLineWithoutSpaces = new StringBuilder();

            foreach (char c in i_Line)
            {
                if (c != ' ')
                {
                    newLineWithoutSpaces.Append(c);
                }
            }

            return newLineWithoutSpaces.ToString();
        }
    }
}
