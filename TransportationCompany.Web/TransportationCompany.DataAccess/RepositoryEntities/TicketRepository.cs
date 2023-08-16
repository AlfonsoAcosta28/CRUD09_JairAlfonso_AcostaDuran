using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess.Repository;

namespace TransportationCompany.DataAccess.RepositoryEntities
{
    public class TicketRepository : Repository<int, Ticket>
    {
        public TicketRepository(TransportationCompanyContext TransportationCompanyContext) : base(TransportationCompanyContext)
        {
        }

     /*   public override async Task<Ticket> AddAsync(Ticket entity)
        {
            //var journey = await Context.Journeys.FindAsync(entity.JourneyId);
            //var passenger = await Context.Passengers.FindAsync(entity.PassengerId);

            var journey =  Context.Journeys.Find(entity.JourneyId);
            var passenger =  Context.Passengers.Find(entity.PassengerId);

            entity.Passenger = null;
            entity.Journey = null;

            await Context.Tickets.AddAsync(entity);

            journey.Tickets.Add(entity);
            passenger.Tickets.Add(entity);

             Context.SaveChanges();
           // await Context.SaveChangesAsync();
            return entity;
        }
     */
        public override async Task<Ticket> GetAsync(int id)
        {
            var ticket = await Context.Tickets
           .Include(t => t.Passenger)
           .Include(t => t.Journey)
               .ThenInclude(j => j.Origin)
           .Include(t => t.Journey)
               .ThenInclude(j => j.Destination)
           .FirstOrDefaultAsync(t => t.Id == id);

            return ticket;
        }

        public override IQueryable<Ticket> GetAll()
        {
            return Context.Tickets
                .Include(t => t.Passenger)
                .Include(t => t.Journey)
                    .ThenInclude(j => j.Origin)
                .Include(t => t.Journey)
                    .ThenInclude(j => j.Destination);
        }


    }
}
