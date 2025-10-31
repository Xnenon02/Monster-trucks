using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Monster_trucks.Data;
using Monster_trucks.Models;

namespace Monster_trucks.Services
{
    public class MonsterTrackerFacade
    {
        private readonly MonsterRepository _monsterRepo;
        private readonly LocationRepository _locationRepo;
        private readonly HunterRepository _hunterRepo;
        private readonly ObservationRepository _observationRepo;
        private readonly string _connectionString;

        public MonsterTrackerFacade(string connectionString)
        {
            _connectionString = connectionString;
            _monsterRepo = new MonsterRepository(connectionString);
            _locationRepo = new LocationRepository(connectionString);
            _hunterRepo = new HunterRepository(connectionString);
            _observationRepo = new ObservationRepository(connectionString);
        }

        // === CRUD: Monster ===
        public void AddMonster(Monster monster) => _monsterRepo.Create(monster);
        public List<Monster> GetAllMonsters() => _monsterRepo.ReadAll();
        public Monster? GetMonsterById(int id) => _monsterRepo.ReadById(id);
        public void UpdateMonster(Monster monster) => _monsterRepo.Update(monster);

        public void DeleteMonster(int id)
        {
            try
            {
                _monsterRepo.Delete(id);
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                Console.WriteLine("❌ Det går inte att ta bort ett monster som har observationer.");
            }
        }

        // === CRUD: Location ===
        public void AddLocation(Location location) => _locationRepo.Create(location);
        public List<Location> GetAllLocations() => _locationRepo.ReadAll();

        // === CRUD: Hunter ===
        public void AddHunter(Hunter hunter) => _hunterRepo.Create(hunter);
        public List<Hunter> GetAllHunters() => _hunterRepo.ReadAll();

        // === CRUD: Observation ===
        public void AddObservation(Observation observation) => _observationRepo.Create(observation);
        public List<Observation> GetAllObservations() => _observationRepo.ReadAll();

        // ======================================================
        // 📊 RAPPORTER & STATISTIK (VG)
        // ======================================================

        // 1️⃣ Mest aktiva monster
        public void ReportMostActiveMonsters()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = @"
                SELECT m.Name, COUNT(o.Id) AS ObsCount
                FROM Observation o
                JOIN Monster m ON o.MonsterId = m.Id
                GROUP BY m.Name
                ORDER BY ObsCount DESC;";

            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("\n=== Mest aktiva monster ===");
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetString(0)} - {reader.GetInt32(1)} observationer");
            }
        }

        // 2️⃣ Farligaste områden
        public void ReportMostDangerousLocations()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = @"
                SELECT l.Name, AVG(m.DangerRating) AS AvgDanger
                FROM Observation o
                JOIN Location l ON o.LocationId = l.Id
                JOIN Monster m ON o.MonsterId = m.Id
                GROUP BY l.Name
                ORDER BY AvgDanger DESC;";

            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("\n=== Farligaste områden ===");
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetString(0)} - Medelfarlighet: {reader.GetDouble(1):0.0}");
            }
        }

        // 3️⃣ Jägarstatistik
        public void ReportHunterStats()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = @"
                SELECT h.Name, COUNT(o.Id) AS ObsCount
                FROM Observation o
                JOIN Hunter h ON o.HunterId = h.Id
                GROUP BY h.Name
                ORDER BY ObsCount DESC;";

            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("\n=== Jägarstatistik ===");
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetString(0)} - {reader.GetInt32(1)} observationer");
            }
        }

        // ======================================================
        // 💾 EXPORT (VG)
        // ======================================================

        public void ExportObservationsToCsv(string filePath)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = @"
                SELECT o.Id, m.Name AS Monster, l.Name AS Location, h.Name AS Hunter, o.ObservedAt, o.Notes
                FROM Observation o
                JOIN Monster m ON o.MonsterId = m.Id
                JOIN Location l ON o.LocationId = l.Id
                JOIN Hunter h ON o.HunterId = h.Id
                ORDER BY o.ObservedAt DESC;";

            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            using var writer = new StreamWriter(filePath);
            writer.WriteLine("Id,Monster,Location,Hunter,ObservedAt,Notes");

            while (reader.Read())
            {
                string notes = reader.IsDBNull(5) ? "" : reader.GetString(5).Replace(",", " ");
                writer.WriteLine($"{reader.GetInt32(0)},{reader.GetString(1)},{reader.GetString(2)},{reader.GetString(3)},{reader.GetString(4)},{notes}");
            }

            Console.WriteLine($"✅ Exporterad till CSV: {filePath}");
        }

        public void ExportObservationsToJson(string filePath)
        {
            var observations = new List<object>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = @"
                SELECT o.Id, m.Name AS Monster, l.Name AS Location, h.Name AS Hunter, o.ObservedAt, o.Notes
                FROM Observation o
                JOIN Monster m ON o.MonsterId = m.Id
                JOIN Location l ON o.LocationId = l.Id
                JOIN Hunter h ON o.HunterId = h.Id
                ORDER BY o.ObservedAt DESC;";

            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                observations.Add(new
                {
                    Id = reader.GetInt32(0),
                    Monster = reader.GetString(1),
                    Location = reader.GetString(2),
                    Hunter = reader.GetString(3),
                    ObservedAt = reader.GetString(4),
                    Notes = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            string json = JsonSerializer.Serialize(observations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

            Console.WriteLine($"✅ Exporterad till JSON: {filePath}");
        }

        // ======================================================
        // 🔄 TRANSAKTION – Flytta observation till ny plats (VG)
        // ======================================================

        public void MoveObservationToNewLocation(int observationId, int newLocationId)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            try
            {
                // Kontrollera att observationen finns
                string checkObs = "SELECT COUNT(*) FROM Observation WHERE Id = @id";
                using (var checkCmd = new SqliteCommand(checkObs, conn, transaction))
                {
                    checkCmd.Parameters.AddWithValue("@id", observationId);
                    long exists = (long)checkCmd.ExecuteScalar();
                    if (exists == 0)
                    {
                        Console.WriteLine($"❌ Ingen observation med ID {observationId} hittades.");
                        transaction.Rollback();
                        return;
                    }
                }

                // Kontrollera att platsen finns
                string checkLoc = "SELECT COUNT(*) FROM Location WHERE Id = @id";
                using (var checkCmd = new SqliteCommand(checkLoc, conn, transaction))
                {
                    checkCmd.Parameters.AddWithValue("@id", newLocationId);
                    long exists = (long)checkCmd.ExecuteScalar();
                    if (exists == 0)
                    {
                        Console.WriteLine($"❌ Ingen plats med ID {newLocationId} hittades.");
                        transaction.Rollback();
                        return;
                    }
                }

                // Hämta gamla platsen (för logg)
                int oldLocId = 0;
                string oldLocSql = "SELECT LocationId FROM Observation WHERE Id = @id";
                using (var oldCmd = new SqliteCommand(oldLocSql, conn, transaction))
                {
                    oldCmd.Parameters.AddWithValue("@id", observationId);
                    var result = oldCmd.ExecuteScalar();
                    if (result != null)
                        oldLocId = Convert.ToInt32(result);
                }

                // Uppdatera observationen
                string updateSql = "UPDATE Observation SET LocationId = @loc WHERE Id = @obs";
                using (var updateCmd = new SqliteCommand(updateSql, conn, transaction))
                {
                    updateCmd.Parameters.AddWithValue("@loc", newLocationId);
                    updateCmd.Parameters.AddWithValue("@obs", observationId);
                    updateCmd.ExecuteNonQuery();
                }

                // Skapa auditlogg-tabellen vid behov
                string createAuditTable = @"
                    CREATE TABLE IF NOT EXISTS AuditLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ObservationId INTEGER,
                        OldLocationId INTEGER,
                        NewLocationId INTEGER,
                        ChangedAt TEXT NOT NULL
                    );";
                using (var createCmd = new SqliteCommand(createAuditTable, conn, transaction))
                {
                    createCmd.ExecuteNonQuery();
                }

                // Lägg till loggrad
                string insertLog = @"
                    INSERT INTO AuditLog (ObservationId, OldLocationId, NewLocationId, ChangedAt)
                    VALUES (@obs, @old, @new, @time);";
                using (var logCmd = new SqliteCommand(insertLog, conn, transaction))
                {
                    logCmd.Parameters.AddWithValue("@obs", observationId);
                    logCmd.Parameters.AddWithValue("@old", oldLocId);
                    logCmd.Parameters.AddWithValue("@new", newLocationId);
                    logCmd.Parameters.AddWithValue("@time", DateTime.UtcNow.ToString("s"));
                    logCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                Console.WriteLine($"✅ Observation #{observationId} flyttades till plats #{newLocationId} och loggades.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ett fel uppstod: {ex.Message}");
                transaction.Rollback();
            }
        }
    }
}
