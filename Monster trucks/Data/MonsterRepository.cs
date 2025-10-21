using Microsoft.Data.Sqlite;
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
        private readonly DatabaseConnection _dbConnection;

        public MonsterRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Create(Monster monster)
        {
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand("INSERT INTO Monster (Name, Type, DangerLevel) VALUES (@name, @type, @dangerLevel)", conn);
            cmd.Parameters.AddWithValue("@name", monster.Name);
            cmd.Parameters.AddWithValue("@type", monster.Type);
            cmd.Parameters.AddWithValue("@dangerLevel", monster.DangerLevel);
            cmd.ExecuteNonQuery();
        }

        public List<Monster> ReadAll()
        {
            var monsters = new List<Monster>();
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand("SELECT Id, Name, Type, DangerLevel FROM Monster", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                monsters.Add(new Monster
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Type = reader.GetString(2),
                    DangerLevel = reader.GetInt32(3)
                });
            }
            return monsters;
        }

        public Monster? ReadById(int id)
        {
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand("SELECT Id, Name, Type, DangerLevel FROM Monster WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Monster
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Type = reader.GetString(2),
                    DangerLevel = reader.GetInt32(3)
                };
            }
            return null;
        }

        public void Update(Monster monster)
        {
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand("UPDATE Monster SET Name = @name, Type = @type, DangerLevel = @dangerLevel WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@name", monster.Name);
            cmd.Parameters.AddWithValue("@type", monster.Type);
            cmd.Parameters.AddWithValue("@dangerLevel", monster.DangerLevel);
            cmd.Parameters.AddWithValue("@id", monster.Id);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _dbConnection.GetConnection();
            using var cmd = new SqliteCommand("DELETE FROM Monster WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
}
