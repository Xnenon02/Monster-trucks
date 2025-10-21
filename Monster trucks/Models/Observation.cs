using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_trucks.Models
{
    using System;

    namespace MonsterTracker.Models
    {
        public class Observation
        {
            public int Id { get; set; }
            public int MonsterId { get; set; }
            public int LocationId { get; set; }
            public int HunterId { get; set; }
            public DateTime ObservedAt { get; set; }  // lägg till
            public string Notes { get; set; }         // lägg till
        }
    }

}
