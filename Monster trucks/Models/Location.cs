using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_trucks.Models;
using Monster_trucks.Data;


namespace Monster_trucks.Models
{
   
        public class Location
        {
            public int Id { get; set; }
            public string Name { get; set; }
           public string Description { get; set; }

            public int DangerLevel { get; set; }     // lägg till
        }
}
