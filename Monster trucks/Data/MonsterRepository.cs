using Microsoft.Data.Sqlite;
using Monster_trucks.Data;
using Monster_trucks.Models;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_trucks.Data
{
    public class MonsterRepository
    {
        private readonly string _connectionString;

        public MonsterRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE
        public void Create(Monster monster, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "INSERT INTO Monster (Name, Species, DangerRating) VALUES (@n, @s, @d)";
                cmd.Parameters.AddWithValue("@n", monster.Name);
                cmd.Parameters.AddWithValue("@s", monster.Species);
                cmd.Parameters.AddWithValue("@d", monster.DangerRating);
                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }

        // READ ALL
        public List<Monster> ReadAll()
        {
            var list = new List<Monster>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Species, DangerRating FROM Monster";
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new Monster
                            {
                                Id = r.GetInt32(0),
                                Name = r.GetString(1),
                                Species = r.GetString(2),
                                DangerRating = r.GetInt32(3)
                            });
                        }
                    }
                }
            }
            return list;
        }

        // GET BY NAME
        public Monster GetByName(string name, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            Monster monster = null;
            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "SELECT Id, Name, Species, DangerRating FROM Monster WHERE Name = @name";
                cmd.Parameters.AddWithValue("@name", name);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        monster = new Monster
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Species = reader.GetString(2),
                            DangerRating = reader.GetInt32(3)
                        };
                    }
                }
            }

            if (ownConn) connection.Close();

            return monster;
        }
        public Monster? ReadById(int id, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            Monster monster = null;
            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "SELECT Id, Name, Species, DangerRating FROM Monster WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        monster = new Monster
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Species = reader.GetString(2),
                            DangerRating = reader.GetInt32(3)
                        };
                    }
                }
            }

            if (ownConn) connection.Close();
            return monster;
        }
        public void Update(Monster monster, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = @"
            UPDATE Monster 
            SET Name = @name, Species = @species, DangerRating = @dangerRating 
            WHERE Id = @id";

                cmd.Parameters.AddWithValue("@name", monster.Name);
                cmd.Parameters.AddWithValue("@species", monster.Species);
                cmd.Parameters.AddWithValue("@dangerRating", monster.DangerRating);
                cmd.Parameters.AddWithValue("@id", monster.Id);

                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }
        public void Delete(int id, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "DELETE FROM Monster WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }




        // COUNT
        public int Count(SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            int count;
            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "SELECT COUNT(*) FROM Monster";
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (ownConn) connection.Close();
            return count;
        }
    }
}
