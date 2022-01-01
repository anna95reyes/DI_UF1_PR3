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

namespace NBA_BD
{
    public class PlayerDB
    {
        public static ObservableCollection<Player> GetLlistaPlayers(int teamId)
        {
            ObservableCollection<Player> players = new ObservableCollection<Player>();

            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        DBUtil.crearParametre(consulta, "@team_id", teamId, DbType.Int32);

                        consulta.CommandText = @"select p.current_number as player_current_number, 
	                                                    p.first_name as player_first_name,
                                                        p.last_name as player_last_name,
                                                        p.photo as player_photo,
                                                        c.name as collage_name,
                                                        p.career_start_year as player_cureer_start_year,
                                                        co.name as country_name, 
                                                        co.short_name as country_short_name,
                                                        p.height as player_height,
                                                        p.weight as player_weight, 
                                                        p.birthday as player_bithday,
                                                        p.position as player_position
                                                 from player p join team t on p.current_team_id = t.id
			                                                  left join college c on p.college_id = c.id
                                                              join country co on p.country_id = co.id
                                                 where t.id = @team_id";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "player_current_number", "player_first_name", "player_last_name", "player_photo",
                                          "collage_name", "player_cureer_start_year", "country_name", "country_short_name",
                                          "player_height", "player_weight", "player_bithday", "player_position"};
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            int player_current_number = reader.GetInt32(ordinals["player_current_number"]);
                            String player_first_name = reader.GetString(ordinals["player_first_name"]);
                            String player_last_name = reader.GetString(ordinals["player_last_name"]);
                            Byte[] player_photo = reader.GetFieldValue<byte[]>(reader.GetOrdinal("player_photo"));
                            String collage_name = readerStringOrNull(reader, ordinals["collage_name"], null);
                            int player_cureer_start_year = reader.GetInt32(ordinals["player_cureer_start_year"]);
                            String country_name = reader.GetString(ordinals["country_name"]);
                            String country_short_name = reader.GetString(ordinals["country_short_name"]);
                            int player_height = reader.GetInt32(ordinals["player_height"]);
                            float player_weight = reader.GetFloat(ordinals["player_weight"]);
                            DateTime player_bithday = reader.GetDateTime(ordinals["player_bithday"]);
                            String player_position = reader.GetString(ordinals["player_position"]);



                            Player p = new Player(player_current_number,player_first_name, player_last_name, player_photo, 
                                                  collage_name, player_cureer_start_year, country_name, country_short_name,
                                                  player_height, player_weight, player_bithday, player_position);
                            players.Add(p);
                        }
                    }
                }
            }
            return players;
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
