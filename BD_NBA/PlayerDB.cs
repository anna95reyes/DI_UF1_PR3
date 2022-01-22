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
                        DBUtil.crearParametre(consulta, "@param_team_id", teamId, DbType.Int32);

                        consulta.CommandText = $@"select p.id as player_id,
                                                         p.current_number as player_current_number, 
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
                                                         p.position+1-1 as player_position
                                                  from player p join team t on p.current_team_id = t.id
			                                                    left join college c on p.college_id = c.id
                                                                join country co on p.country_id = co.id
                                                  where t.id = @param_team_id";

                        DbDataReader reader = consulta.ExecuteReader(); //per cuan pot retorna mes d'una fila

                        Dictionary<string, int> ordinals = new Dictionary<string, int>();
                        string[] cols = { "player_id","player_current_number", "player_first_name", "player_last_name", 
                                          "player_photo", "collage_name", "player_cureer_start_year", "country_name", 
                                          "country_short_name", "player_height", "player_weight", "player_bithday", 
                                          "player_position"};
                        foreach (string c in cols)
                        {
                            ordinals[c] = reader.GetOrdinal(c);
                        }

                        while (reader.Read()) //llegeix la fila seguent, retorna true si ha pogut llegir la fila, retorna false si no hi ha mes dades per lleguir
                        {
                            int player_id = reader.GetInt32(ordinals["player_id"]);
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
                            int player_position = reader.GetInt32(ordinals["player_position"]);



                            Player p = new Player(teamId, player_id, player_current_number,player_first_name, player_last_name, player_photo, 
                                                  new College(collage_name), player_cureer_start_year, new Country (country_name, country_short_name),
                                                  player_height, player_weight, player_bithday, player_position);
                            players.Add(p);
                        }
                    }
                }
            }
            return players;
        }

        public static bool delete(int playerId)
        {
            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                bool haAnatBe = true;

                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    DbTransaction transaccio = connection.BeginTransaction(); //Creacio d'una transaccio

                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.Transaction = transaccio; // marques la consulta dins de la transacció


                        DBUtil.crearParametre(consulta, "@player_id", playerId, DbType.Int32);
                        consulta.CommandText = "select count(1) from player where id = @player_id";
                        long numEmpleats = (long)consulta.ExecuteScalar();

                        if (numEmpleats != 1) return false;

                        consulta.CommandText = "delete from player where id = @player_id";

                        int numDeleted = consulta.ExecuteNonQuery();

                        if (numDeleted != 1)
                        {
                            transaccio.Rollback();
                            haAnatBe = false;
                        }
                        transaccio.Commit();
                        return haAnatBe;

                    }

                }

            }
        }

        public static void insert(Player p)
        {
            //TODO: fer l'insert
            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    DbTransaction transaccio = connection.BeginTransaction(); //Creacio d'una transaccio

                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.Transaction = transaccio; // marques la consulta dins de la transacció

                        consulta.CommandText = "select max(id)+1 from player";
                        int nextPlayerId = (int)(Int64)consulta.ExecuteScalar();

                        DBUtil.crearParametre(consulta, "@player_id", nextPlayerId, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@country_name", p.Country.Name, DbType.String);
                        DBUtil.crearParametre(consulta, "@country_short_name", p.Country.ShortName, DbType.String);
                        DBUtil.crearParametre(consulta, "@collage_name", p.College.Name, DbType.String);

                        consulta.CommandText = $@"select id 
                                                  from country 
                                                  where name = @country_name and short_name = @country_short_name";
                        int countryId = (int)consulta.ExecuteScalar();

                        DBUtil.crearParametre(consulta, "@country_id", countryId, DbType.Int32);

                        consulta.CommandText = $@"select id from college where name = @collage_name";
                        int collegeId = (int)consulta.ExecuteScalar();

                        DBUtil.crearParametre(consulta, "@college_id", collegeId, DbType.Int32);
                        /*
                        public Player(int teamId, int playerId, int playerCurrentNumber, string playerFirstName, 
                        string playerLastName, byte[] playerPhoto, College collageName, int playerCareerStartYear, 
                        Country country, int playerHeight, float playerWeight, DateTime playerBithday, int playerPosition)

                         */

                        //select id from country where name = "USA" and short_name = "us"

                        DBUtil.crearParametre(consulta, "@team_id", p.TeamId, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_current_number", p.PlayerCurrentNumber, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_first_name", p.PlayerFirstName, DbType.String);
                        DBUtil.crearParametre(consulta, "@player_last_name", p.PlayerLastName, DbType.String);
                        //reader.GetFieldValue<byte[]>
                        DBUtil.crearParametre(consulta, "@player_photo", p.PlayerPhoto, DbType.Binary);
                        DBUtil.crearParametre(consulta, "@player_career_start_year", p.PlayerCareerStartYear, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_height", p.PlayerHeight, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_weight", p.PlayerWeight, DbType.Decimal);
                        DBUtil.crearParametre(consulta, "@player_bithday", p.PlayerBithday, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_position", p.PlayerPosition, DbType.Int32);
                        
                        consulta.CommandText = $@"insert into player (id, first_name, last_name, photo, career_start_year, 
                                                                    career_end_year, country_id, height, birthday, weight, 
                                                                    college_id, current_team_id, current_number, rating, 
                                                                    nba_profile, photo_play, position) 
                                                  values (@player_id, @player_first_name, @player_last_name, @player_photo,
                                                          @player_career_start_year, null, @country_id, @player_height,
                                                          @player_bithday, @player_weight, @college_id, @team_id,
                                                          @player_current_number, null, null, null, @player_position)";

                        int numeroDeFiles = consulta.ExecuteNonQuery(); //per fer un update o un delete
                        if (numeroDeFiles != 1)
                        {
                            //shit happens
                            transaccio.Rollback();
                        }
                        else
                        {
                            p.PlayerId = (int)nextPlayerId;
                            transaccio.Commit();
                        }

                    }

                }
            }
        }

        public static void update(Player p)
        {
            //TODO: fer l'update
            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    DbTransaction transaccio = connection.BeginTransaction(); //Creacio d'una transaccio

                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.Transaction = transaccio; // marques la consulta dins de la transacció


                        DBUtil.crearParametre(consulta, "@player_id", p.PlayerId, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@country_name", p.Country.Name, DbType.String);
                        DBUtil.crearParametre(consulta, "@country_short_name", p.Country.ShortName, DbType.String);
                        DBUtil.crearParametre(consulta, "@collage_name", p.College.Name, DbType.String);

                        consulta.CommandText = $@"select id 
                                                  from country 
                                                  where name = @country_name and short_name = @country_short_name";
                        int countryId = (int)consulta.ExecuteScalar();

                        DBUtil.crearParametre(consulta, "@country_id", countryId, DbType.Int32);

                        consulta.CommandText = $@"select id from college where name = @collage_name";
                        int collegeId = (int)consulta.ExecuteScalar();

                        DBUtil.crearParametre(consulta, "@college_id", collegeId, DbType.Int32);
                        
                        DBUtil.crearParametre(consulta, "@team_id", p.TeamId, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_current_number", p.PlayerCurrentNumber, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_first_name", p.PlayerFirstName, DbType.String);
                        DBUtil.crearParametre(consulta, "@player_last_name", p.PlayerLastName, DbType.String);
                        //reader.GetFieldValue<byte[]>
                        DBUtil.crearParametre(consulta, "@player_photo", p.PlayerPhoto, DbType.Binary);
                        DBUtil.crearParametre(consulta, "@player_career_start_year", p.PlayerCareerStartYear, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_height", p.PlayerHeight, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@player_weight", p.PlayerWeight, DbType.Decimal);
                        DBUtil.crearParametre(consulta, "@player_bithday", p.PlayerBithday, DbType.DateTime);
                        DBUtil.crearParametre(consulta, "@player_position", p.PlayerPosition, DbType.Int32);
                        
                        consulta.CommandText = $@"update player set first_name = @player_first_name,
                                                                    last_name = @player_last_name,
                                                                    photo = @player_photo,
                                                                    career_start_year = @player_career_start_year,
                                                                    country_id =  @country_id,
                                                                    height = @player_height,
                                                                    birthday = @player_bithday,
                                                                    weight = @player_weight,
                                                                    college_id = @college_id,
                                                                    current_number = @player_current_number,
                                                                    position = @player_position
                                                  where id = @player_id";

                        int numeroDeFiles = consulta.ExecuteNonQuery(); //per fer un update o un delete
                        if (numeroDeFiles != 1)
                        {
                            //shit happens
                            transaccio.Rollback();
                        }
                        else
                        {
                            transaccio.Commit();
                        }

                    }

                }
            }
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
