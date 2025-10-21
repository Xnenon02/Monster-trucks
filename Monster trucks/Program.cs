using System;
using System.IO;
using Microsoft.Data.Sqlite;

using Monster_trucks.Data;
using Monster_trucks.Services;

namespace Monster_trucks
{
    class Program
    {
        static void Main()
        {
            // Get the folder where the executable runs (bin/Debug/netX.Y)
            string exePath = AppDomain.CurrentDomain.BaseDirectory;

            // Go up 3 levels to project root folder
            string projectRoot = Path.GetFullPath(Path.Combine(exePath, @"..\..\.."));

            // Define folder for the database inside the project (e.g. "Data")
            string dbFolder = Path.Combine(projectRoot, "Data");

            // Make sure the folder exists
            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            // Full path to the DB file
            string dbPath = Path.Combine(dbFolder, "monstertracker.db");
            Console.WriteLine("Using DB at: " + dbPath);


            // Connection string with the full path
            string connectionString = $"Data Source={dbPath}";

            Console.WriteLine("Using DB at: " + dbPath);

            // Initialize and run your database connection and repositories with the dynamic connection string
            var dbConnection = new DatabaseConnection(connectionString);
            dbConnection.InitializeDatabase();

            var monsterRepo = new MonsterRepository(connectionString);
            var locationRepo = new LocationRepository(connectionString);
            var hunterRepo = new HunterRepository(connectionString);
            var observationRepo = new ObservationRepository(connectionString);

            var seeder = new DatabaseSeeder(monsterRepo, locationRepo, hunterRepo, observationRepo, connectionString);
            seeder.Seed();
        }
    }
}
