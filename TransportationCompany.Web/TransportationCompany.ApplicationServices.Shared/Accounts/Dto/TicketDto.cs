using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.Accounts.Dto
{
    public class TicketDto
    {
        [Key]
        public int Id { get; set; }
        public int JourneyId { get; set; }
        public int PassengerId { get; set; }
        public int Seat { get; set; }
       // public Journey Journey { get; set; }
     //   public Passenger Passenger { get; set; }
    }
}
