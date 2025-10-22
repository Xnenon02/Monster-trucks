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
            // === 1️⃣ Hitta rätt sökväg för databasen ===
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(exePath, @"..\..\.."));
            string dbFolder = Path.Combine(projectRoot, "Data");

            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            string dbPath = Path.Combine(dbFolder, "monstertracker.db");
            string connectionString = $"Data Source={dbPath}";

            Console.WriteLine("Använder databas: " + dbPath);

            // === 2️⃣ Initiera databas och repositories ===
            var dbConnection = new DatabaseConnection(connectionString);
            dbConnection.InitializeDatabase();

            var monsterRepo = new MonsterRepository(connectionString);
            var locationRepo = new LocationRepository(connectionString);
            var hunterRepo = new HunterRepository(connectionString);
            var observationRepo = new ObservationRepository(connectionString);

            var seeder = new DatabaseSeeder(monsterRepo, locationRepo, hunterRepo, observationRepo, connectionString);
            seeder.Seed();

            // === 3️⃣ Starta enkel meny ===
            RunMenu(monsterRepo, locationRepo, hunterRepo);
        }

        static void RunMenu(
            MonsterRepository monsterRepo,
            LocationRepository locationRepo,
            HunterRepository hunterRepo)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== MONSTER TRACKER ===");
                Console.WriteLine("1. Visa alla jägare");
                Console.WriteLine("2. Visa alla platser");
                Console.WriteLine("3. Visa alla monster");
                Console.WriteLine("0. Avsluta");
                Console.Write("Välj ett alternativ: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowHunters(hunterRepo);
                        break;
                    case "2":
                        ShowLocations(locationRepo);
                        break;
                    case "3":
                        ShowMonsters(monsterRepo);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val!");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                }
            }
        }

        // === Visa jägare ===
        static void ShowHunters(HunterRepository repo)
        {
            var hunters = repo.ReadAll();
            Console.Clear();
            Console.WriteLine("=== JÄGARE ===");
            if (hunters.Count == 0)
            {
                Console.WriteLine("Inga jägare hittades.");
                return;
            }

            foreach (var h in hunters)
            {
                Console.WriteLine($"Namn: {h.Name}");
                Console.WriteLine($"Erfarenhetsnivå: {h.ExperienceLevel}");
                Console.WriteLine($"Kontakt: {h.ContactInfo}\n");
            }
        }

        // === Visa platser ===
        static void ShowLocations(LocationRepository repo)
        {
            var locations = repo.ReadAll();
            Console.Clear();
            Console.WriteLine("=== PLATSER ===");
            if (locations.Count == 0)
            {
                Console.WriteLine("Inga platser hittades.");
                return;
            }

            foreach (var l in locations)
            {
                Console.WriteLine($"Plats: {l.Name}");
                Console.WriteLine($"Beskrivning: {l.Description}");
                Console.WriteLine($"Farlighetsnivå: {l.DangerLevel}\n");
            }
        }

        // === Visa monster ===
        static void ShowMonsters(MonsterRepository repo)
        {
            var monsters = repo.ReadAll();
            Console.Clear();
            Console.WriteLine("=== MONSTER ===");
            if (monsters.Count == 0)
            {
                Console.WriteLine("Inga monster hittades.");
                return;
            }

            foreach (var m in monsters)
            {
                Console.WriteLine($"Namn: {m.Name}");
                Console.WriteLine($"Art: {m.Species}");
                Console.WriteLine($"Farlighetsgrad: {m.DangerRating}/5\n");
            }
        }
    }
}
