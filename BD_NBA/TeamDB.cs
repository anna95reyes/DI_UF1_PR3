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
    public class TeamDB
    {
        public static ObservableCollection<Team> GetLlistaTeams(Division divisionCaption, string teamCaption)
        {
            ObservableCollection<Team> teams = new ObservableCollection<Team>();

            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        String division = (divisionCaption != null)? divisionCaption.ToString() : "";
                        DBUtil.crearParametre(consulta, "@param_division_caption", division, DbType.String);
                        DBUtil.crearParametre(consulta, "@param_team_caption", teamCaption, DbType.String);
                        DBUtil.crearParametre(consulta, "@param_team_caption_like", "%" + teamCaption + "%", DbType.String);

                        consulta.CommandText = @"select t.id as team_id, 
                                                        t.caption as team_caption, 
                                                        t.short_caption as team_short_name,
                                                        d.caption as division_caption, 
                                                        t.logo as team_logo, 
                                                        a.caption as arena_caption, 
                                                        a.logo as arena_logo,
                                                        a.about as arena_about,
                                                        a.capacity as arena_capacity, 
                                                        a.photo as arena_photo,
                                                        a.lat as arena_lat, 
                                                        a.lng as arena_long
                                                 from team t join division d on t.current_division_id = d.id
			                                                 join arena a on t.arena_id = a.id
                                                 where (@param_team_caption='' OR upper(t.caption) like upper(@param_team_caption_like))
                                                   and (@param_division_caption='' or @param_division_caption = d.caption)
                                                 order by t.caption";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "team_id", "team_caption", "team_short_name", "division_caption", "team_logo",
                                          "arena_caption", "arena_logo", "arena_about", "arena_capacity",
                                          "arena_photo", "arena_lat", "arena_long"};
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            int team_id = reader.GetInt32(ordinals["team_id"]);
                            string team_caption = reader.GetString(ordinals["team_caption"]);
                            string team_short_name = reader.GetString(ordinals["team_short_name"]);
                            string division_caption = reader.GetString(ordinals["division_caption"]);
                            String team_logo = reader.GetString(ordinals["team_logo"]);
                            String arena_caption = reader.GetString(ordinals["arena_caption"]);
                            string arena_logo = reader.GetString(ordinals["arena_logo"]);
                            string arena_about = reader.GetString(ordinals["arena_about"]);
                            int arena_capacity = reader.GetInt32(ordinals["arena_capacity"]);
                            string arena_photo = reader.GetString(ordinals["arena_photo"]);
                            double arena_lat = reader.GetDouble(ordinals["arena_lat"]);
                            double arena_long = reader.GetDouble(ordinals["arena_long"]);

                            Team te = new Team(team_id, team_caption, team_short_name, new Division(division_caption), team_logo, arena_caption,
                                                           arena_logo, arena_about, arena_capacity, arena_photo, arena_lat, arena_long);
                            teams.Add(te);
                        }
                    }
                }
            }
            return teams;
        }
    }
}
