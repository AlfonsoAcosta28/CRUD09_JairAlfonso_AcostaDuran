using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportationCompany.Core.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Journey> DestinationJourneys { get; set; }    
        public List<Journey> OriginJourneys { get; set; }

        public City() {
            DestinationJourneys = new List<Journey>();
            OriginJourneys = new List<Journey>();
        }
    }
}
