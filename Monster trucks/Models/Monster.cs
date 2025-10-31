using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_trucks.Models;
using Monster_trucks.Data;

namespace Monster_trucks.Models
{
    public class Monster
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Ursprunglig egenskap
        public string Species { get; set; } = string.Empty;

        // Lägg till alias så UI och SQL inte kraschar
        public string Type
        {
            get => Species;
            set => Species = value;
        }

        public int DangerRating { get; set; }
    }
}
