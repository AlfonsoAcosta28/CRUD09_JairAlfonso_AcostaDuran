using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.DataAccess
{
    public class TransportationCompanyContext : IdentityDbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Passengers> Passengers { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public TransportationCompanyContext(DbContextOptions<TransportationCompanyContext> options) : base(options) { }
    }
}
