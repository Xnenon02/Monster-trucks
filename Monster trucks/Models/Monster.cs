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
        public string Name { get; set; }
        public string Species { get; set; }     // lägg till
        public int DangerRating { get; set; }   // lägg till
    }
}
