using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_trucks.Models;

namespace Monster_trucks.Data
{
    public class LocationRepository
    {
        private readonly string _connectionString;

        public LocationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE
        public void Create(Location location, SqliteConnection connection = null, SqliteTransaction transaction = null)
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
                cmd.CommandText = "INSERT INTO Locations (Name, Description, DangerLevel) VALUES (@n, @d, @l)";
                cmd.Parameters.AddWithValue("@n", location.Name);
                cmd.Parameters.AddWithValue("@d", location.Description ?? "");
                cmd.Parameters.AddWithValue("@l", location.DangerLevel);
                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }

        // READ ALL
        public List<Location> ReadAll()
        {
            var list = new List<Location>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Description, DangerLevel FROM Locations";
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new Location
                            {
                                Id = r.GetInt32(0),
                                Name = r.GetString(1),
                                Description = r.GetString(2),
                                DangerLevel = r.GetInt32(3)
                            });
                        }
                    }
                }
            }
            return list;
        }

        // GET BY NAME
        public Location GetByName(string name, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            Location loc = null;
            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "SELECT Id, Name, Description, DangerLevel FROM Locations WHERE Name = @n";
                cmd.Parameters.AddWithValue("@n", name);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        loc = new Location
                        {
                            Id = r.GetInt32(0),
                            Name = r.GetString(1),
                            Description = r.GetString(2),
                            DangerLevel = r.GetInt32(3)
                        };
                    }
                }
            }

            if (ownConn) connection.Close();
            return loc;
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
                cmd.CommandText = "SELECT COUNT(*) FROM Locations";
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (ownConn) connection.Close();
            return count;
        }
    }
}
