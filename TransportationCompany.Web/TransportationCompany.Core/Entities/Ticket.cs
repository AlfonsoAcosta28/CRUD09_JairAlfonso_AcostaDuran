using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportationCompany.Core.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public Journey JourneyId { get; set; }

        public Passengers PassengerId { get; set; }
    }
}
