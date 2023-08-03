using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess.Repository;

namespace TransportationCompany.DataAccess.RepositoryEntities
{
    public class PassengersRepository : Repository<int, Passengers>
    {
        public PassengersRepository(TransportationCompanyContext TransportationCompanyContext) : base(TransportationCompanyContext)
        {
        }
    }
}
