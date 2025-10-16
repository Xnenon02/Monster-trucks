using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_trucks.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public int MonsterId { get; set; }
        public int LocationId { get; set; }
        public int HunterId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; } = null!;
    }
}
