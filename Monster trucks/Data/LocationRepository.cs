using Microsoft.Data.Sqlite;
using Monster_trucks.Models;
using System;
using System.Collections.Generic;

namespace Monster_trucks.Data
{
    public class LocationRepository
    {
        private readonly string _connectionString;

        public LocationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // === CREATE ===
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
                cmd.CommandText = @"
                    INSERT INTO Location (Name, Description, DangerLevel)
                    VALUES (@n, @d, @l)";
                cmd.Parameters.AddWithValue("@n", location.Name);
                cmd.Parameters.AddWithValue("@d", location.Description ?? "");
                cmd.Parameters.AddWithValue("@l", location.DangerLevel);
                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }

        // === READ ALL ===
        public List<Location> ReadAll()
        {
            var list = new List<Location>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Description, DangerLevel FROM Location";
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new Location
                            {
                                Id = r.GetInt32(0),
                                Name = r.GetString(1),
                                Description = r.IsDBNull(2) ? "" : r.GetString(2),
                                DangerLevel = r.GetInt32(3)
                            });
                        }
                    }
                }
            }
            return list;
        }

        // === READ BY ID ===
        public Location? ReadById(int id, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            Location location = null;
            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "SELECT Id, Name, Description, DangerLevel FROM Location WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        location = new Location
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            DangerLevel = reader.GetInt32(3)
                        };
                    }
                }
            }

            if (ownConn) connection.Close();
            return location;
        }

        // === READ BY NAME ===
        public Location? GetByName(string name, SqliteConnection connection = null, SqliteTransaction transaction = null)
        {
            bool ownConn = connection == null;
            if (ownConn)
            {
                connection = new SqliteConnection(_connectionString);
                connection.Open();
            }

            Location location = null;
            using (var cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "SELECT Id, Name, Description, DangerLevel FROM Location WHERE Name = @n";
                cmd.Parameters.AddWithValue("@n", name);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        location = new Location
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            DangerLevel = reader.GetInt32(3)
                        };
                    }
                }
            }

            if (ownConn) connection.Close();
            return location;
        }

        // === UPDATE ===
        public void Update(Location location, SqliteConnection connection = null, SqliteTransaction transaction = null)
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
                    UPDATE Location
                    SET Name = @n, Description = @d, DangerLevel = @l
                    WHERE Id = @id";
                cmd.Parameters.AddWithValue("@n", location.Name);
                cmd.Parameters.AddWithValue("@d", location.Description ?? "");
                cmd.Parameters.AddWithValue("@l", location.DangerLevel);
                cmd.Parameters.AddWithValue("@id", location.Id);
                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }

        // === DELETE ===
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
                cmd.CommandText = "DELETE FROM Location WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }

        // === COUNT ===
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
                cmd.CommandText = "SELECT COUNT(*) FROM Location";
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (ownConn) connection.Close();
            return count;
        }
    }
}
