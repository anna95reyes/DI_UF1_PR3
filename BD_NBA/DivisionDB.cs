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
    public class DivisionDB
    {
        public static ObservableCollection<Division> GetLlistaDivision()
        {
            ObservableCollection<Division> divisions = new ObservableCollection<Division>();

            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.CommandText = $@"select caption
                                                  from division
                                                  order by caption";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "caption"};
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            String caption = reader.GetString(ordinals["caption"]);

                            Division d = new Division(caption);
                            divisions.Add(d);
                        }
                    }
                }
            }
            return divisions;
        }
    }
}
