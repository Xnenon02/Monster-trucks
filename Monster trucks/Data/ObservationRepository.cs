using Microsoft.Data.Sqlite;
using Monster_trucks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_trucks.Data
{
    public class ObservationRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public ObservationRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Create(Observation observation)
        {
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand(@"
                INSERT INTO Observation (MonsterId, LocationId, HunterId, Date, Details) 
                VALUES (@monsterId, @locationId, @hunterId, @date, @details)", conn);

            cmd.Parameters.AddWithValue("@monsterId", observation.MonsterId);
            cmd.Parameters.AddWithValue("@locationId", observation.LocationId);
            cmd.Parameters.AddWithValue("@hunterId", observation.HunterId);
            cmd.Parameters.AddWithValue("@date", observation.Date);
            cmd.Parameters.AddWithValue("@details", observation.Details);

            cmd.ExecuteNonQuery();
        }

        public List<Observation> ReadAll()
        {
            var observations = new List<Observation>();
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand("SELECT Id, MonsterId, LocationId, HunterId, Date, Details FROM Observation", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                observations.Add(new Observation
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    MonsterId = Convert.ToInt32(reader["MonsterId"]),
                    LocationId = Convert.ToInt32(reader["LocationId"]),
                    HunterId = Convert.ToInt32(reader["HunterId"]),
                    Date = Convert.ToDateTime(reader["Date"]),
                    Details = reader["Details"].ToString()
                });
            }

            return observations;
        }

        // Update och Delete metoder följer samma princip
    }
}

