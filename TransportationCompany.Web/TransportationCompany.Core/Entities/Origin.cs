using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportationCompany.Core.Entities
{
    public class Origin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Journey> Journeys { get; set; }

        public Origin()
        {
            Journeys = new List<Journey>();
        }
    }
}
