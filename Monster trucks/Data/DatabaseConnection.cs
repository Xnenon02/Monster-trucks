using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Monster_trucks.Models;

namespace Monster_trucks.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InitializeDatabase()
        {
            Console.WriteLine("🔧 Running InitializeDatabase...");

            // Find project root relative to executable location
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(exePath, @"..\..\.."));

            // Full path to the schema file
            string schemaPath = Path.Combine(projectRoot, "monstertracker_schema.sql");

            if (File.Exists(schemaPath))
            {
                string sql = File.ReadAllText(schemaPath);

                // Split commands by ';' and remove empty entries
                string[] commands = sql.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                foreach (var commandText in commands)
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = commandText.Trim();
                    if (!string.IsNullOrWhiteSpace(command.CommandText))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("✅ Database initialized successfully.");
            }
            else
            {
                Console.WriteLine($"⚠️ Could not find monstertracker_schema.sql at: {schemaPath}");
            }
        }

        public SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
