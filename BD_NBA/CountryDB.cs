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
    public class CountryDB
    {
        public static ObservableCollection<Country> GetLlistaCountry()
        {
            ObservableCollection<Country> countries = new ObservableCollection<Country>();

            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.CommandText = @"select name
                                                 from country
                                                 order by name";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "name" };
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            String name = reader.GetString(ordinals["name"]);

                            Country c = new Country(name);
                            countries.Add(c);
                        }
                    }
                }
            }
            return countries;
        }

        public static String GetShortCountry(String nameCountry)
        {
            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        DBUtil.crearParametre(consulta, "@country_name", nameCountry, DbType.String);

                        consulta.CommandText = @"select short_name
                                                from country
                                                where name = @country_name";

                        return (String)consulta.ExecuteScalar(); //per cuan pot retorna una i nomes una fila

                    }
                }
            }
        }
    }
}
