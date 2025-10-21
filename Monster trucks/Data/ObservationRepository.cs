using Microsoft.Data.Sqlite;
using Monster_trucks.Models;
using Monster_trucks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_trucks.Data
{
    public class ObservationRepository
    {
        private readonly string _connectionString;

        public ObservationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE
        public void Create(Observation observation, SqliteConnection connection = null, SqliteTransaction transaction = null)
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
                    INSERT INTO Observations (MonsterId, LocationId, HunterId, ObservedAt, Notes)
                    VALUES (@m, @l, @h, @o, @n)";
                cmd.Parameters.AddWithValue("@m", observation.MonsterId);
                cmd.Parameters.AddWithValue("@l", observation.LocationId);
                cmd.Parameters.AddWithValue("@h", observation.HunterId);
                cmd.Parameters.AddWithValue("@o", observation.ObservedAt);
                cmd.Parameters.AddWithValue("@n", observation.Notes ?? "");
                cmd.ExecuteNonQuery();
            }

            if (ownConn) connection.Close();
        }

        // READ ALL
        public List<Observation> ReadAll()
        {
            var list = new List<Observation>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, MonsterId, LocationId, HunterId, ObservedAt, Notes FROM Observations";
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new Observation
                            {
                                Id = r.GetInt32(0),
                                MonsterId = r.GetInt32(1),
                                LocationId = r.GetInt32(2),
                                HunterId = r.GetInt32(3),
                                ObservedAt = r.GetDateTime(4),
                                Notes = r.GetString(5)
                            });
                        }
                    }
                }
            }
            return list;
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
                cmd.CommandText = "SELECT COUNT(*) FROM Observations";
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (ownConn) connection.Close();
            return count;
        }
    }
}

