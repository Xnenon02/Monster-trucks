using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Monster_trucks.Models;
using Monster_trucks.Services;

namespace Monster_trucks.UI
{
    public class ConsoleUI
    {
        private readonly MonsterTrackerFacade _facade;

        public ConsoleUI(MonsterTrackerFacade facade)
        {
            _facade = facade;
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== 🧟 MONSTER TRACKER – Grimville ===");
                Console.ResetColor();

                Console.WriteLine("1. Visa alla monster");
                Console.WriteLine("2. Visa alla platser");
                Console.WriteLine("3. Visa alla jägare");
                Console.WriteLine("4. Visa alla observationer");
                Console.WriteLine("5. 📊 Rapporter & statistik");
                Console.WriteLine("6. 💾 Exportera data (CSV/JSON)");
                Console.WriteLine("7. 🔄 Flytta observation (Transaktion)");
                Console.WriteLine("0. Avsluta");
                Console.Write("\nVälj ett alternativ: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ShowMonsters();
                        break;
                    case "2":
                        ShowLocations();
                        break;
                    case "3":
                        ShowHunters();
                        break;
                    case "4":
                        ShowObservations();
                        break;
                    case "5":
                        ShowReportsMenu();
                        break;
                    case "6":
                        ShowExportMenu();
                        break;
                    case "7":
                        MoveObservationMenu();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val!");
                        Console.ResetColor();
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
                    Console.ReadKey();
                }
            }
        }

        // === CRUD-visning ===
        private void ShowMonsters()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== MONSTER ===");
            Console.ResetColor();

            var monsters = _facade.GetAllMonsters();
            if (monsters.Count == 0)
            {
                Console.WriteLine("Inga monster hittades.");
                return;
            }

            foreach (var m in monsters)
            {
                Console.ForegroundColor = m.DangerRating switch
                {
                    >= 4 => ConsoleColor.Red,
                    3 => ConsoleColor.Yellow,
                    _ => ConsoleColor.Green
                };

                Console.WriteLine($"#{m.Id}: {m.Name} ({m.Type}) – Farlighetsnivå: {m.DangerRating}/5");
                Console.ResetColor();
            }
        }

        private void ShowLocations()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== PLATSER ===");
            Console.ResetColor();

            var locations = _facade.GetAllLocations();
            if (locations.Count == 0)
            {
                Console.WriteLine("Inga platser hittades.");
                return;
            }

            foreach (var l in locations)
                Console.WriteLine($"#{l.Id}: {l.Name} – Område: {l.Description}");
        }

        private void ShowHunters()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== JÄGARE ===");
            Console.ResetColor();

            var hunters = _facade.GetAllHunters();
            if (hunters.Count == 0)
            {
                Console.WriteLine("Inga jägare hittades.");
                return;
            }

            foreach (var h in hunters)
                Console.WriteLine($"#{h.Id}: {h.Name} ({h.ExperienceLevel}) – Kontakt: {h.ContactInfo}");
        }

        private void ShowObservations()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== OBSERVATIONER ===");
            Console.ResetColor();

            var observations = _facade.GetAllObservations();
            if (observations.Count == 0)
            {
                Console.WriteLine("Inga observationer hittades.");
                return;
            }

            foreach (var o in observations)
            {
                Console.WriteLine($"Observation #{o.Id}");
                Console.WriteLine($"MonsterId: {o.MonsterId}");
                Console.WriteLine($"PlatsId: {o.LocationId}");
                Console.WriteLine($"JägareId: {o.HunterId}");
                Console.WriteLine($"Datum: {o.ObservedAt}");
                Console.WriteLine($"Anteckningar: {o.Notes}\n");
            }
        }

        // === RAPPORT-MENY ===
        private void ShowReportsMenu()
        {
            bool inReports = true;
            while (inReports)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== 📊 RAPPORTER & STATISTIK ===");
                Console.ResetColor();

                Console.WriteLine("1. Mest aktiva monster");
                Console.WriteLine("2. Farligaste områden");
                Console.WriteLine("3. Jägarstatistik");
                Console.WriteLine("0. Tillbaka");
                Console.Write("\nVälj en rapport: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        _facade.ReportMostActiveMonsters();
                        break;
                    case "2":
                        Console.Clear();
                        _facade.ReportMostDangerousLocations();
                        break;
                    case "3":
                        Console.Clear();
                        _facade.ReportHunterStats();
                        break;
                    case "0":
                        inReports = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val!");
                        Console.ResetColor();
                        break;
                }

                if (inReports)
                {
                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                }
            }
        }

        // === EXPORT-MENY ===
        private void ShowExportMenu()
        {
            bool inExport = true;
            while (inExport)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== 💾 EXPORTERA DATA ===");
                Console.ResetColor();

                Console.WriteLine("1. Exportera observationer till CSV");
                Console.WriteLine("2. Exportera observationer till JSON");
                Console.WriteLine("0. Tillbaka");
                Console.Write("\nVälj ett alternativ: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ExportCsv();
                        break;
                    case "2":
                        ExportJson();
                        break;
                    case "0":
                        inExport = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val!");
                        Console.ResetColor();
                        break;
                }

                if (inExport)
                {
                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                }
            }
        }

        private void ExportCsv()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "observationer.csv");
            _facade.ExportObservationsToCsv(filePath);
        }

        private void ExportJson()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "observationer.json");
            _facade.ExportObservationsToJson(filePath);
        }

        // === 🔄 FLYTTA OBSERVATION (TRANSAKTION) ===
        private void MoveObservationMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== 🔄 Flytta observation ===");
            Console.ResetColor();

            Console.Write("Ange observationens ID: ");
            if (!int.TryParse(Console.ReadLine(), out int obsId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Felaktigt ID.");
                Console.ResetColor();
                return;
            }

            Console.Write("Ange nya platsens ID: ");
            if (!int.TryParse(Console.ReadLine(), out int newLocId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Felaktigt ID.");
                Console.ResetColor();
                return;
            }

            _facade.MoveObservationToNewLocation(obsId, newLocId);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Operation slutförd (se eventuella loggmeddelanden ovan).");
            Console.ResetColor();
        }
    }
}
