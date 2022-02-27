using Microsoft.EntityFrameworkCore;
using NBA_BD.db;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Text;

namespace NBA_BD
{
    public class PlayOffDB
    {
        public static ObservableCollection<PlayOff> GetLlistaSeasons(String Season, int Round, String Conference)
        {
            ObservableCollection<PlayOff> playOffs = new ObservableCollection<PlayOff>();

            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        DBUtil.crearParametre(consulta, "@season", Season, DbType.String);
                        DBUtil.crearParametre(consulta, "@round", Round, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@conference", Conference, DbType.String);

                        consulta.CommandText = $@"  select se1.seed as seed_first_team,
                                                           tm1.caption as first_team_name, 
                                                           tm1.logo as first_team_logo, 
                                                           se2.seed as seed_second_team, 
                                                           tm2.caption as second_team_name, 
                                                           tm2.logo as second_team_logo,
                                                           po.first_team_wins as first_team_wins,
                                                           po.second_team_wins as second_team_wins
                                                    from playoff po join team tm1 on po.first_team_id = tm1.id
				                                                    join team tm2 on po.second_team_id = tm2.id
                                                                    join season s on po.season = s.year
                                                                    left join conference c on po.conference_id = c.id
                                                                    join seed se1 on tm1.id = se1.team_id
                                                                    join seed se2 on tm2.id = se2.team_id
                                                    where (@conference='' OR upper(c.caption) like upper(@conference))
                                                          and po.round_id = @round
                                                          and s.caption = @season
                                                    order by po.round_id";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "first_team_name", "second_team_name","first_team_logo", "second_team_logo", "seed_first_team", 
                                          "seed_second_team", "first_team_wins", "second_team_wins"};
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            String first_team_name = reader.GetString(ordinals["first_team_name"]);
                            String second_team_name = reader.GetString(ordinals["second_team_name"]);
                            String first_team_logo = reader.GetString(ordinals["first_team_logo"]);
                            String second_team_logo = reader.GetString(ordinals["second_team_logo"]);
                            int seed_first_team = reader.GetInt32(ordinals["seed_first_team"]);
                            int seed_second_team = reader.GetInt32(ordinals["seed_second_team"]);
                            int first_team_wins = reader.GetInt32(ordinals["first_team_wins"]);
                            int second_team_wins = reader.GetInt32(ordinals["second_team_wins"]);

                            PlayOff s = new PlayOff(first_team_name, second_team_name, first_team_logo, second_team_logo, seed_first_team, seed_second_team,
                                                    first_team_wins, second_team_wins, Conference, Round, Season);
                            playOffs.Add(s);
                        }
                    }
                }
            }
            return playOffs;
        }
    }
}
