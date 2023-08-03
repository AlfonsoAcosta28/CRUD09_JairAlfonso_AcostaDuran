using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportationCompany.Core.Entities
{
    public class Destination
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Journey> Journeys { get; set; }

        public Destination()
        {
            Journeys = new List<Journey>();
        }
    }
}
