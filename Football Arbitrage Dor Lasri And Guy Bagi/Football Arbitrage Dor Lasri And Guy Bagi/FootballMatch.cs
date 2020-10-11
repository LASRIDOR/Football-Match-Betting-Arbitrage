using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    // To-Do : overridew ToString
    public class FootballMatch
    {
        private string m_FirstTeam;
        private string m_SecondTeam;
        private float m_FirstTeamRatio;
        private float m_SecondTeamRatio;
        private float m_FirstTeamIAP = 0;
        private float m_SecondTeamIAP = 0;
        private float m_FirstTeamGamble = 0;
        private float m_SecondTeamGamble = 0;
        private string m_StatsUrl;
        private string m_MatchStats;

        public string FirstTeam
        {
            get { return m_FirstTeam; }
        }
        public string SecondTeam
        {
            get { return m_SecondTeam; }
        }
        public float FirstTeamRatio
        {
            get { return m_FirstTeamRatio; }
        }
        public float SecondTeamRatio
        {
            get { return m_SecondTeamRatio; }
        }
        public float FirstTeamGamble
        {
            get { return m_FirstTeamGamble; }
            set { m_FirstTeamGamble = value; }
        }
        public float SecondTeamGamble
        {
            get { return m_SecondTeamGamble; }
            set { m_SecondTeamGamble = value; }
        }
        public float FirstTeamIAP
        {
            get { return m_FirstTeamIAP; }
            set { m_FirstTeamIAP = value; }
        }
        public float SecondTeamIAP
        {
            get { return m_SecondTeamIAP; }
            set { m_SecondTeamIAP = value; }
        }

        public string MatchStats
        {
            get { return m_MatchStats; }
            set { m_MatchStats = value; }
        }

        public string StatsUrl
        {
            get { return m_StatsUrl; }
            set { m_StatsUrl = value; }
        }

        public FootballMatch(string FirstTeam, float FirstTeamRatio, string SecondTeam, float SecondTeamRatio, string StatUrl)
        {
            m_FirstTeam = FirstTeam;
            m_FirstTeamRatio = FirstTeamRatio;
            m_SecondTeam = SecondTeam;
            m_SecondTeamRatio = SecondTeamRatio;
            m_StatsUrl = StatUrl;
        }
    }
}
