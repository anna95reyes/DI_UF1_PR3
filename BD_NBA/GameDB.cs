using Microsoft.EntityFrameworkCore;
using NBA_BD.db;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace NBA_BD
{
    public class GameDB
    {
        public static ObservableCollection<Game> GetLlistGames(String Season, int Round, String TeamHome, String TeamAway)
        {
            ObservableCollection<Game> games = new ObservableCollection<Game>();

            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        DBUtil.crearParametre(consulta, "@season", Season, DbType.String);
                        DBUtil.crearParametre(consulta, "@round", Round, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@team_home", TeamHome, DbType.String);
                        DBUtil.crearParametre(consulta, "@team_away", TeamAway, DbType.String);

                        consulta.CommandText = $@"select g.game_date as game_date,
                                                  th.logo as home_team, 
                                                  sum(gq.home_team_score) as home_score, 
                                                  sum(gq.away_team_score) as away_score, 
                                                  ta.logo as away_team, g.recap as recap
                                                  from game g join team th on g.home_team_id = th.id
		                                                      join team ta on g.away_team_id = ta.id
		                                                      join game_quarter gq on g.id = gq.game_id
                                                              join season s on g.season_id = s.year
                                                  where s.caption = @season and
	                                                    th.caption in (@team_home, @team_away) and
                                                        ta.caption in (@team_home, @team_away) and
                                                        g.round_id = @round
                                                  group by g.game_date, th.logo, ta.logo, g.recap
                                                  order by g.game_date";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "game_date", "home_team", "home_score", "away_score", "away_team", "recap"};
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            DateTime game_date = reader.GetDateTime(ordinals["game_date"]);
                            String home_team = reader.GetString(ordinals["home_team"]);
                            int home_score = reader.GetInt32(ordinals["home_score"]);
                            int away_score = reader.GetInt32(ordinals["away_score"]);
                            String away_team = reader.GetString(ordinals["away_team"]);
                            String recap = readerStringOrNull(reader, ordinals["recap"], "");

                            Game g = new Game(game_date, home_team, home_score, away_score, away_team, recap);

                            games.Add(g);
                        }
                    }
                }
            }
            return games;
        }

        private static string readerStringOrNull(DbDataReader reader, int ordinal, String valorPerDefecte)
        {
            string value = valorPerDefecte;
            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetString(ordinal);
            }
            return value;
        }
    }
}
