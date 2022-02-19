using System;
using System.Collections.Generic;
using System.Text;

namespace NBA_BD.Model
{
    public class Game
    {
        private DateTime gameDate;
        private String homeTeam;
        private int homeScore;
        private int awayScore;
        private String awayTeam;
        private String recap;
        private String sGameDate;

        public Game(DateTime gameDate, string homeTeam, int homeScore, int awayScore, string awayTeam, string recap)
        {
            GameDate = gameDate;
            HomeTeam = homeTeam;
            HomeScore = homeScore;
            AwayScore = awayScore;
            AwayTeam = awayTeam;
            Recap = recap;
            SGameDate = gameDate.ToString("dd MMM yyyy");
        }

        public DateTime GameDate { get => gameDate; set => gameDate = value; }
        public string HomeTeam { get => homeTeam; set => homeTeam = value; }
        public int HomeScore { get => homeScore; set => homeScore = value; }
        public int AwayScore { get => awayScore; set => awayScore = value; }
        public string AwayTeam { get => awayTeam; set => awayTeam = value; }
        public string Recap { get => recap; set => recap = value; }
        public string SGameDate { get => sGameDate; set => sGameDate = value; }

    }
}
