using Microsoft.Data.Sqlite;
using Monster_trucks.Data;
using Monster_trucks.Models;

using System;

namespace Monster_trucks.Services
{
    public class DatabaseSeeder
    {
        private readonly MonsterRepository _monsterRepo;
        private readonly LocationRepository _locationRepo;
        private readonly HunterRepository _hunterRepo;
        private readonly ObservationRepository _observationRepo;
        private readonly string _connectionString;

        public DatabaseSeeder(
            MonsterRepository monsterRepo,
            LocationRepository locationRepo,
            HunterRepository hunterRepo,
            ObservationRepository observationRepo,
            string connectionString)
        {
            _monsterRepo = monsterRepo;
            _locationRepo = locationRepo;
            _hunterRepo = hunterRepo;
            _observationRepo = observationRepo;
            _connectionString = connectionString;
        }

        public void Seed()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // --- Locations ---
                        var locations = new[]
                        {
                            new Location { Name = "Svartskogen", Description = "Tät skog, dimma ofta", DangerLevel = 3 },
                            new Location { Name = "Öde Tjärn", Description = "Säregen sjö, kall", DangerLevel = 4 },
                            new Location { Name = "Klipphamn", Description = "Hamnområde, ruiner", DangerLevel = 2 },
                        };

                        foreach (var loc in locations)
                        {
                            if (_locationRepo.GetByName(loc.Name, connection, transaction) == null)
                            {
                                _locationRepo.Create(loc, connection, transaction);
                            }
                        }

                        // --- Monsters ---
                        var monsters = new[]
                        {
                            new Monster { Name = "Nattalv", Species = "Alviform", DangerRating = 2 },
                            new Monster { Name = "Tjärnvarelse", Species = "Anomali", DangerRating = 5 },
                            new Monster { Name = "Stenklo", Species = "Golem", DangerRating = 3 },
                        };

                        foreach (var m in monsters)
                        {
                            if (_monsterRepo.GetByName(m.Name, connection, transaction) == null)
                            {
                                _monsterRepo.Create(m, connection, transaction);
                            }
                        }

                        // --- Hunters ---
                        var hunters = new[]
                        {
                            new Hunter { Name = "Anna J.", ExperienceLevel = 4 },
                            new Hunter { Name = "Lars B.", ExperienceLevel = 2 },
                            new Hunter { Name = "Maja K.", ExperienceLevel = 5 },
                        };

                        foreach (var h in hunters)
                        {
                            if (_hunterRepo.GetByName(h.Name, connection, transaction) == null)
                            {
                                _hunterRepo.Create(h, connection, transaction);
                            }
                        }

                        // --- Observations ---
                        if (_observationRepo.Count(connection, transaction) == 0)
                        {
                            var svartskogen = _locationRepo.GetByName("Svartskogen", connection, transaction);
                            var odeTjarn = _locationRepo.GetByName("Öde Tjärn", connection, transaction);

                            var nattalv = _monsterRepo.GetByName("Nattalv", connection, transaction);
                            var tjarnv = _monsterRepo.GetByName("Tjärnvarelse", connection, transaction);

                            var anna = _hunterRepo.GetByName("Anna J.", connection, transaction);
                            var lars = _hunterRepo.GetByName("Lars B.", connection, transaction);

                            var observations = new[]
                            {
                                new Observation
                                {
                                    MonsterId = nattalv.Id,
                                    LocationId = svartskogen.Id,
                                    HunterId = anna.Id,
                                    ObservedAt = DateTime.UtcNow.AddDays(-7),
                                    Notes = "Rörde sig snabbt i skogsbrynet."
                                },
                                new Observation
                                {
                                    MonsterId = tjarnv.Id,
                                    LocationId = odeTjarn.Id,
                                    HunterId = lars.Id,
                                    ObservedAt = DateTime.UtcNow.AddDays(-3),
                                    Notes = "Stor våg och märkligt ljus."
                                },
                                new Observation
                                {
                                    MonsterId = nattalv.Id,
                                    LocationId = svartskogen.Id,
                                    HunterId = lars.Id,
                                    ObservedAt = DateTime.UtcNow.AddDays(-1),
                                    Notes = "Såg flera individer."
                                }
                            };

                            foreach (var obs in observations)
                            {
                                _observationRepo.Create(obs, connection, transaction);
                            }
                        }

                        transaction.Commit();
                        Console.WriteLine("✅ Databasen seedad med testdata.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Fel vid seeding: {ex.Message}");
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
