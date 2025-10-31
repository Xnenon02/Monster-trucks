using Microsoft.Data.Sqlite;
using Monster_trucks.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_trucks.Data
{
    public class HunterRepository
    {
        private readonly string _connectionString;

        public HunterRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE
        public void Create(Hunter hunter, SqliteConnection connection = null, SqliteTransaction transaction = null)
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
                cmd.CommandText = "INSERT INTO Hunter (Name, ExperienceLevel) VALUES (@n, @e)";
                cmd.Parameters.AddWithValue("@n", hunter.Name);
                cmd.Parameters.AddWithValue("@e", hunter.ExperienceLevel);
                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }

        // READ ALL
        public List<Hunter> ReadAll()
        {
            var list = new List<Hunter>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, ExperienceLevel FROM Hunter";
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new Hunter
                            {
                                Id = r.GetInt32(0),
                                Name = r.GetString(1),
                                ExperienceLevel = r.GetInt32(2)
                            });
                        }
                    }
                }
            }
            return list;
        }

        // GET BY NAME
        public Hunter GetByName(string name, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            Hunter hunter = null;
            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "SELECT Id, Name, ExperienceLevel FROM Hunter WHERE Name = @n";
                cmd.Parameters.AddWithValue("@n", name);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hunter = new Hunter
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ExperienceLevel = reader.GetInt32(2)
                        };
                    }
                }
            }

            if (ownConn) connection.Close();

            return hunter;
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
                cmd.CommandText = "SELECT COUNT(*) FROM Hunter";
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (ownConn) connection.Close();
            return count;
        }
    }
}

