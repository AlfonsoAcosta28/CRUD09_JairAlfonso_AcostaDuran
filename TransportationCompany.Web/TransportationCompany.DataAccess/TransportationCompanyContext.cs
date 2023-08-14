
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.DataAccess
{
    public class TransportationCompanyContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public TransportationCompanyContext(DbContextOptions<TransportationCompanyContext> options) : base(options)
        {
        }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Journey>()
                .HasOne(j => j.Origin)
                .WithMany(c => c.OriginJourneys)
                .HasForeignKey(j => j.OriginId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Journey>()
                .HasOne(j => j.Destination)
                .WithMany(c => c.DestinationJourneys)
                .HasForeignKey(j => j.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Journey)
                .WithMany(j => j.Tickets)
                .HasForeignKey(t => t.JourneyId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Passenger)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PassengerId);
            
        }
    }
}
