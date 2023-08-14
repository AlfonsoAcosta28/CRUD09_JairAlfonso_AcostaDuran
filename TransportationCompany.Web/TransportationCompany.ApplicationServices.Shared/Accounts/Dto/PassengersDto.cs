using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.Accounts.Dto
{
    public class PassengersDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(32)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(32)]
        public string LastName { get; set; }
        public int Age { get; set; }

        public List<Ticket> Tickets { get; set; }

        public PassengersDto()
        {
            Tickets = new List<Ticket>();
        }
    }
}
