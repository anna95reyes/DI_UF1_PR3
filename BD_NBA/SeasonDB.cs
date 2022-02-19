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
    public class SeasonDB
    {
        public static ObservableCollection<Season> GetLlistaSeasons()
        {
            ObservableCollection<Season> seasons = new ObservableCollection<Season>();

            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.CommandText = $@"select year, caption 
                                                  from season 
                                                  order by year desc";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "year","caption" };
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            String year = reader.GetString(ordinals["year"]);
                            String caption = reader.GetString(ordinals["caption"]);

                            Season s = new Season(year, caption);
                            seasons.Add(s);
                        }
                    }
                }
            }
            return seasons;
        }
    }
}
