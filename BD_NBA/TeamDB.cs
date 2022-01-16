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

                        consulta.CommandText = $@"select t.id as team_id, 
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

        public static void insert(Team t)
        {
            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    DbTransaction transaccio = connection.BeginTransaction(); //Creacio d'una transaccio

                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.Transaction = transaccio; // marques la consulta dins de la transacció

                        consulta.CommandText = "select max(id)+1 from team";
                        int nextTeamId = (int)(Int64)consulta.ExecuteScalar();

                        consulta.CommandText = "select max(id)+1 from arena";
                        int nextArenaId = (int)(Int64)consulta.ExecuteScalar();

                        DBUtil.crearParametre(consulta, "@team_id", nextTeamId, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@arena_id", nextArenaId, DbType.Int32);
                        
                        DBUtil.crearParametre(consulta, "@team_caption", t.TeamCaption, DbType.String);
                        DBUtil.crearParametre(consulta, "@team_short_name", t.TeamShortCaption, DbType.String);
                        DBUtil.crearParametre(consulta, "@division_caption", t.DivisionCaption.Caption, DbType.String);
                        DBUtil.crearParametre(consulta, "@team_logo", t.TeamLogo, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_caption", t.ArenaCaption, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_logo", t.ArenaLogo, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_about", t.ArenaAbout, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_capacity", t.ArenaCapacity, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@arena_photo", t.ArenaPhoto, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_lat", t.ArenaLat, DbType.Double);
                        DBUtil.crearParametre(consulta, "@arena_long", t.ArenaLong, DbType.Double);

                        consulta.CommandText = "select id from division where caption = @division_caption";
                        int DivisionId = (int)consulta.ExecuteScalar();
                        if (DivisionId < 0)
                        {
                            transaccio.Rollback();
                            throw new Exception("No s'ha pogut recuperar la Division");
                        }

                        DBUtil.crearParametre(consulta, "@division_id", DivisionId, DbType.Int32);

                        consulta.CommandText = $@"insert into arena (id, caption, lat, lng, logo, about, photo, capacity)  
                                                  values (@arena_id, @arena_caption, @arena_lat, @arena_long, @arena_logo,
                                                         @arena_about, @arena_photo, @arena_capacity)";
                        int numeroDeFilesArena = consulta.ExecuteNonQuery(); //per fer un update o un delete

                        consulta.CommandText = $@"insert into team (id, caption, logo, year_founded, website, short_caption, about, current_division_id, arena_id)  
                                                  values (@team_id, @team_caption, @team_logo, 1900, '', @team_short_name, '', @division_id, @arena_id)";
                        int numeroDeFilesTeam = consulta.ExecuteNonQuery(); //per fer un update o un delete

                        if (numeroDeFilesArena != 1 && numeroDeFilesTeam != 1)
                        {
                            //shit happens
                            transaccio.Rollback();
                        }
                        else
                        {
                            t.TeamId = (int)nextTeamId;
                            transaccio.Commit();
                        }

                    }

                }

            }
        }

        public static void update(Team t)
        {
            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    DbTransaction transaccio = connection.BeginTransaction(); //Creacio d'una transaccio

                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.Transaction = transaccio; // marques la consulta dins de la transacció

                        DBUtil.crearParametre(consulta, "@team_id", t.TeamId, DbType.Int32);
                        
                        consulta.CommandText = "select arena_id from team where id = @team_id";
                        int arenaId = (int)consulta.ExecuteScalar();

                        DBUtil.crearParametre(consulta, "@arena_id", arenaId, DbType.Int32);

                        DBUtil.crearParametre(consulta, "@team_caption", t.TeamCaption, DbType.String);
                        DBUtil.crearParametre(consulta, "@team_short_name", t.TeamShortCaption, DbType.String);
                        DBUtil.crearParametre(consulta, "@division_caption", t.DivisionCaption.Caption, DbType.String);
                        DBUtil.crearParametre(consulta, "@team_logo", t.TeamLogo, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_caption", t.ArenaCaption, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_logo", t.ArenaLogo, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_about", t.ArenaAbout, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_capacity", t.ArenaCapacity, DbType.Int32);
                        DBUtil.crearParametre(consulta, "@arena_photo", t.ArenaPhoto, DbType.String);
                        DBUtil.crearParametre(consulta, "@arena_lat", t.ArenaLat, DbType.Double);
                        DBUtil.crearParametre(consulta, "@arena_long", t.ArenaLong, DbType.Double);

                        consulta.CommandText = "select id from division where caption = @division_caption";
                        int DivisionId = (int)consulta.ExecuteScalar();
                        if (DivisionId < 0)
                        {
                            transaccio.Rollback();
                            throw new Exception("No s'ha pogut recuperar la Division");
                        }

                        DBUtil.crearParametre(consulta, "@division_id", DivisionId, DbType.Int32);

                        consulta.CommandText = $@"update arena 
                                                  set caption = @arena_caption, 
                                                      lat = @arena_lat,
                                                      lng = @arena_long, 
                                                      logo = @arena_logo, 
                                                      about = @arena_about, 
                                                      photo = @arena_photo, 
                                                      capacity = @arena_capacity
                                                  where id = @arena_id";
                        int numeroDeFilesArena = consulta.ExecuteNonQuery(); //per fer un update o un delete

                        consulta.CommandText = $@"update team 
                                                  set caption = @team_caption,
                                                      logo = @team_logo, 
                                                      short_caption = @team_short_name, 
                                                      current_division_id = @division_id
                                                  where id = @team_id";
                        int numeroDeFilesTeam = consulta.ExecuteNonQuery(); //per fer un update o un delete

                        if (numeroDeFilesArena != 1 && numeroDeFilesTeam != 1)
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

        public static bool delete(int teamId)
        {
            bool haAnatBe = true;
            using (MySqlDBContext context = new MySqlDBContext()) //crea el contexte de la base de dades
            {
                using (DbConnection connection = context.Database.GetDbConnection()) //pren la conexxio de la BD
                {
                    connection.Open();
                    DbTransaction transaccio = connection.BeginTransaction(); //Creacio d'una transaccio

                    using (DbCommand consulta = connection.CreateCommand())
                    {
                        consulta.Transaction = transaccio; // marques la consulta dins de la transacció


                        DBUtil.crearParametre(consulta, "@team_id", teamId, DbType.Int32);
                        consulta.CommandText = "select count(1) from team where id = @team_id";
                        long numEmpleats = (long)consulta.ExecuteScalar();

                        if (numEmpleats != 1) return false;

                        consulta.CommandText = "delete from team where id = @team_id";

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
    }
}
