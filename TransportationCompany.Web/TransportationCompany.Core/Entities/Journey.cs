using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportationCompany.Core.Entities
{
    public class Journey
    {
        [Key]
        public int Id { get; set; }

        public Destination DestinationId { get; set; }
        public Origin OriginId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }

        public List<Ticket> Tickets { get; set; }

        public Journey()
        {
            Tickets = new List<Ticket>();
        }
    }
}
