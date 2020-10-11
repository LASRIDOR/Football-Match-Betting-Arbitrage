using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Arbitrage
{
    abstract public class Scraper
    {
        private readonly string r_WebsiteUrl;
        private readonly HtmlWeb r_HtmlWeb;
        private HtmlDocument m_HtmlDocument;
        public delegate void ScraperConnectionDelegate(object sender, string i_Url);
        public event ScraperConnectionDelegate m_ToDoWhenFailConnection;

        public string WebsiteUrl
        {
            get { return r_WebsiteUrl; }
        }

        public HtmlWeb HtmlWeb
        {
            get { return r_HtmlWeb; }
        }

        public HtmlDocument HtmlDocument
        {
            get { return m_HtmlDocument; }
            set { m_HtmlDocument = value; }
        }

        public Scraper(string WebsiteUrl)
        {
            r_WebsiteUrl = WebsiteUrl;
            r_HtmlWeb = new HtmlWeb();
            m_ToDoWhenFailConnection = null;
        }

        public event ScraperConnectionDelegate ToDoWhenFailConnection {
            add { m_ToDoWhenFailConnection += value; }
            remove { m_ToDoWhenFailConnection -= value; }
        }

        public void OnFailConnection(object sender, string i_Url)
        {
            // lets tell the form that I was clicked:
            if (m_ToDoWhenFailConnection != null)
            {
                m_ToDoWhenFailConnection.Invoke(this, i_Url);
            }
            else
            {
                Console.WriteLine(i_Url + "Has No Action On Fail Connection");
            }
        }

        public void AddActionDelegate(ScraperConnectionDelegate ActionDelegate)
        {
            ToDoWhenFailConnection += ActionDelegate;
        }

        // To-Do : throw Exception/event in case of connection not good
        public void LoadUrl()
        {
            try
            {
                HtmlDocument = HtmlWeb.Load(WebsiteUrl);
            }
            catch(Exception e)
            {
                Console.WriteLine("an Error Occured: " + e.Message);

                if (e is System.UriFormatException)
                {
                    m_ToDoWhenFailConnection.Invoke(this, this.r_WebsiteUrl);
                }

                throw new Exception(WebsiteUrl + " were unable to connect to server");
            }
        }

        //abstract public void LoadUrl(); // throw Exception/event in case of connection not good
        abstract public List<FootballMatch> MakeListOfDailyMatchesPlaying();
        abstract public string StatsCollector(string m_StatsUrl);
        abstract public string ToString();
        abstract public int GetHashCode();
        //abstract public void AddActionDelegate(ScraperConnectionDelegate ActionDelegate);
    }
}
