using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.Accounts.Dto
{
    public class JourneyDto
    {
        [Key]
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public int OriginId { get; set; }
        //public City Destination { get; set; }
        //public City Origin { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }

        public List<Ticket> Tickets { get; set; }

        public JourneyDto()
        {
            Tickets = new List<Ticket>();
        }
    }
}
