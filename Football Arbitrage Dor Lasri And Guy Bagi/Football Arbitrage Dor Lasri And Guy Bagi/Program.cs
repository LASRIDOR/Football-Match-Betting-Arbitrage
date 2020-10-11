using Arbitrage;
using HtmlAgilityPack;
using System;
using Quartz;
using System.Collections.Specialized;
using Quartz.Impl;

namespace WebScraper
{
    public class Program
    {
        public static void Main()
        {
            SystemManager manager = new SystemManager();

            manager.OpenSystemManagerForArbitrageFounder();

            //"https://www.winner.co.il/mainbook/sport-%D7%9B%D7%93%D7%95%D7%A8%D7%92%D7%9C?&marketTypePeriod=1%7C100"

            Console.WriteLine("Arbitrager Scraper is Done Come Back Later");
            Console.ReadLine();
        }
    }
}