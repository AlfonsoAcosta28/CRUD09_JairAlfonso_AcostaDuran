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

        public override async Task<Ticket> AddAsync(Ticket entity)
        {
            var journey = await Context.Journeys.FindAsync(entity.Journey.Id);
            var passenger = await Context.Passengers.FindAsync(entity.Passenger.Id);

            entity.Passenger = null;
            entity.Journey = null;

            await Context.Ticket.AddAsync(entity);

            journey.Tickets.Add(entity);
            passenger.Tickets.Add(entity);

            await Context.SaveChangesAsync();
            return entity;
        }
/*
        public override async Task<Ticket> GetAsync(int id)
        {
            var journey = await Context.Journeys.Include(x => x.Tickets).FirstOrDefaultAsync(x => x.Id == id);
            var passenger = await Context.Passengers.Include(x => x.Tickets).FirstOrDefaultAsync(x => x.Id == id);

                 
        }
*/
    }
}
