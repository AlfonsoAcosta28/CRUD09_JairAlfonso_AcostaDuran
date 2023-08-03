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

        public City DestinationId { get; set; }
        public City OriginId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }

        public List<Ticket> Tickets { get; set; }

        public JourneyDto()
        {
            Tickets = new List<Ticket>();
        }
    }
}
