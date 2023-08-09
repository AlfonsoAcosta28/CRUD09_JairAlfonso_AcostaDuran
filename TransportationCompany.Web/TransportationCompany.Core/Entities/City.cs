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

        List<Journey> Journeys { get; set; }    

        public City() { 
            Journeys = new List<Journey>();
        }
    }
}
