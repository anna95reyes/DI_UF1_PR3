using System;
using System.Collections.Generic;
using System.Text;

namespace NBA_BD.Model
{
    public class PlayOff
    {
        private String firstTeamName;
        private String secondTeamName;
        private String firstTeamLogo;
        private String secondTeamLogo;
        private int seedFirstTeam;
        private int seedSecondTeam;
        private int firstTeamWins;
        private int secondTeamWins;
        private String conference;
        private int round;
        private String season;

        public PlayOff(string firstTeamName, string secondTeamName, string firstTeamLogo, string secondTeamLogo, int seedFirstTeam, int seedSecondTeam, int firstTeamWins, int secondTeamWins, string conference, int round, string season)
        {
            FirstTeamName = firstTeamName;
            SecondTeamName = secondTeamName;
            FirstTeamLogo = firstTeamLogo;
            SecondTeamLogo = secondTeamLogo;
            SeedFirstTeam = seedFirstTeam;
            SeedSecondTeam = seedSecondTeam;
            FirstTeamWins = firstTeamWins;
            SecondTeamWins = secondTeamWins;
            Conference = conference;
            Round = round;
            Season = season;
        }

        public string FirstTeamName { get => firstTeamName; set => firstTeamName = value; }
        public string SecondTeamName { get => secondTeamName; set => secondTeamName = value; }
        public string FirstTeamLogo { get => firstTeamLogo; set => firstTeamLogo = value; }
        public string SecondTeamLogo { get => secondTeamLogo; set => secondTeamLogo = value; }
        public int SeedFirstTeam { get => seedFirstTeam; set => seedFirstTeam = value; }
        public int SeedSecondTeam { get => seedSecondTeam; set => seedSecondTeam = value; }
        public int FirstTeamWins { get => firstTeamWins; set => firstTeamWins = value; }
        public int SecondTeamWins { get => secondTeamWins; set => secondTeamWins = value; }
        public string Conference { get => conference; set => conference = value; }
        public int Round { get => round; set => round = value; }
        public string Season { get => season; set => season = value; }
        




        /*
            select se1.seed, tm1.logo, se2.seed, tm2.logo, po.first_team_wins, po.second_team_wins, c.caption, po.round_id
            from playoff po join team tm1 on po.first_team_id = tm1.id
				            join team tm2 on po.second_team_id = tm2.id
                            join season s on po.season = s.year
                            left join conference c on po.conference_id = c.id
                            join seed se1 on tm1.id = se1.team_id
                            join seed se2 on tm2.id = se2.team_id
            where s.caption = "2013 - 2014 NBA season"
            order by po.round_id;
         */

        /*
            select tm1.id, se1.seed, tm1.logo, tm2.id, se2.seed, tm2.logo, po.first_team_wins, po.second_team_wins, c.caption, po.round_id
            from playoff po join team tm1 on po.first_team_id = tm1.id
				            join team tm2 on po.second_team_id = tm2.id
                            join season s on po.season = s.year
                            left join conference c on po.conference_id = c.id
                            join seed se1 on tm1.id = se1.team_id
                            join seed se2 on tm2.id = se2.team_id
            where c.caption is null or (c.caption is not null and c.caption = "Western")
            and  po.round_id = 4
            and s.caption = "2013 - 2014 NBA season"
            order by po.round_id
         */

    }
}
