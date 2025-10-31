using System;
using System.IO;
using Monster_trucks.Data;
using Monster_trucks.Services;
using Monster_trucks.UI;

namespace Monster_trucks
{
    class Program
    {
        static void Main()
        {
            // === 1️⃣ Hitta rätt sökväg för databasen ===
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(exePath, @"..\..\.."));
            string dbFolder = Path.Combine(projectRoot, "Data");

            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            string dbPath = Path.Combine(dbFolder, "monstertracker.db");
            string connectionString = $"Data Source={dbPath}";

            Console.WriteLine("Använder databas: " + dbPath);

            // === 2️⃣ Initiera databas och seed ===
            var dbConnection = new DatabaseConnection(connectionString);
            dbConnection.InitializeDatabase();

            var monsterRepo = new MonsterRepository(connectionString);
            var locationRepo = new LocationRepository(connectionString);
            var hunterRepo = new HunterRepository(connectionString);
            var observationRepo = new ObservationRepository(connectionString);

            var seeder = new DatabaseSeeder(monsterRepo, locationRepo, hunterRepo, observationRepo, connectionString);
            seeder.Seed();

            // === 3️⃣ Skapa Facade och UI ===
            var facade = new MonsterTrackerFacade(connectionString);
            var ui = new ConsoleUI(facade);

            // === 4️⃣ Starta programmet ===
            ui.Run();

            Console.WriteLine("\nProgrammet avslutas. Tack för att du använde Monster Tracker!");
        }
    }
}
