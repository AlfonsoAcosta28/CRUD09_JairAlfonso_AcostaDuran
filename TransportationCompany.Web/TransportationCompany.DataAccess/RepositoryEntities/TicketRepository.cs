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
    }
}
