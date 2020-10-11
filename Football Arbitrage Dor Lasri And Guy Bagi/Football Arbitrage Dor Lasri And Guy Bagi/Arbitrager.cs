using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    public static class Arbitrager
    {
        private static float CalculateIAP(float odd)
        {
            return ((1 / odd) * 100);
        }

        public static bool isArbitrage(ref FootballMatch match)
        {
            bool v_IsArbitrage;

            match.FirstTeamIAP = CalculateIAP(match.FirstTeamRatio);
            match.SecondTeamIAP = CalculateIAP(match.SecondTeamRatio);
            v_IsArbitrage = (match.FirstTeamIAP + match.SecondTeamIAP) < 100;

            return v_IsArbitrage;
        }

        public static void GamblingRatio(ref FootballMatch match)
        {
            match.FirstTeamGamble = match.FirstTeamIAP / (match.FirstTeamIAP + match.SecondTeamIAP);
            match.SecondTeamGamble = match.SecondTeamIAP / (match.FirstTeamIAP + match.SecondTeamIAP);
        }
    }
}