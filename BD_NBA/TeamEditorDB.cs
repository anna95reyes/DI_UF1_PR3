using Microsoft.EntityFrameworkCore;
using NBA_BD.db;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;

namespace NBA_BD
{
    public class TeamEditorDB
    {
        public static ObservableCollection<TeamEditor> GetLlistaTeams()
        {
            ObservableCollection<TeamEditor> teams = new ObservableCollection<TeamEditor>();

            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.CommandText = @"select t.logo as team_logo, t.caption as team, d.caption as division, a.caption as arena
                                                 from team t join division d on t.current_division_id = d.id
			                                                 join arena a on t.arena_id = a.id
                                                 order by t.caption;";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "team_logo", "team", "division", "arena" };
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            String team_logo = reader.GetString(ordinals["team_logo"]);
                            string team = reader.GetString(ordinals["team"]);
                            string division = reader.GetString(ordinals["division"]);
                            string arena = reader.GetString(ordinals["arena"]);

                            TeamEditor te = new TeamEditor(team_logo, team, division, arena);
                            teams.Add(te);
                        }
                    }
                }
            }
            return teams;
        }
    }
}
