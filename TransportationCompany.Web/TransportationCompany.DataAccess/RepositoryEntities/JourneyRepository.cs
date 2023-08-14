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
    public class JourneyRepository : Repository<int, Journey>
    {
        public JourneyRepository(TransportationCompanyContext TransportationCompanyContext) : base(TransportationCompanyContext)
        {
        }
        public override async Task<Journey> AddAsync(Journey entity)
        {
            //var origin = await Context.Cities.FindAsync(entity.OriginId);
            //var destination = await Context.Cities.FindAsync(entity.DestinationId);

            var origin =  Context.Cities.Find(entity.OriginId);
            var destination =  Context.Cities.Find(entity.DestinationId);

            entity.Destination = null;
            entity.Origin = null;

            await Context.Journeys.AddAsync(entity);

            origin.OriginJourneys.Add(entity);
            destination.DestinationJourneys.Add(entity);

            //await Context.SaveChangesAsync();
             Context.SaveChanges();
            return entity;
        }
        public override async Task<Journey> GetAsync(int id)
        {
           return await Context.Journeys.Include(j => j.Origin).Include(j => j.Destination).FirstOrDefaultAsync(j => j.Id == id);
        }

        public override IQueryable<Journey> GetAll()
        {
            return Context.Journeys.Include(j => j.Origin).Include(j => j.Destination);
        }

    }
}
