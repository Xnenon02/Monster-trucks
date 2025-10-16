using Monster_trucks.Data;
using Monster_trucks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_trucks.Services
{
    public class MonsterTrackerFacade
    {
        private readonly MonsterRepository _monsterRepo;
        private readonly LocationRepository _locationRepo;
        private readonly HunterRepository _hunterRepo;
        private readonly ObservationRepository _observationRepo;

        public MonsterTrackerFacade(DatabaseConnection dbConnection)
        {
            _monsterRepo = new MonsterRepository(dbConnection);
            _locationRepo = new LocationRepository(dbConnection);
            _hunterRepo = new HunterRepository(dbConnection);
            _observationRepo = new ObservationRepository(dbConnection);
        }

        // Monster
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
            catch (Exception ex)
            {
                // Hantera FK-constraint fel
                throw new Exception("Det går inte att ta bort monster som har observationer.", ex);
            }
        }

        // Liknande metoder för Location, Hunter, Observation - du kan implementera på samma sätt

        // Exempel Location
        public void AddLocation(Location location) => _locationRepo.Create(location);
        public List<Location> GetAllLocations() => _locationRepo.ReadAll();

        // ... osv
    }
}

