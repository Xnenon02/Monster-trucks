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
        private readonly DatabaseConnection _dbConnection;

        public HunterRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Create(Hunter hunter)
        {
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand("INSERT INTO Hunter (Name, ContactInfo) VALUES (@name, @contact)", conn);
            cmd.Parameters.AddWithValue("@name", hunter.Name);
            cmd.Parameters.AddWithValue("@contact", hunter.ContactInfo);
            cmd.ExecuteNonQuery();
        }

        public List<Hunter> ReadAll()
        {
            var hunters = new List<Hunter>();
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand("SELECT Id, Name, ContactInfo FROM Hunter", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                hunters.Add(new Hunter
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    ContactInfo = reader["ContactInfo"].ToString()
                });
            }

            return hunters;
        }

        // Lägg till Update och Delete metoder här också
    }
}
}
