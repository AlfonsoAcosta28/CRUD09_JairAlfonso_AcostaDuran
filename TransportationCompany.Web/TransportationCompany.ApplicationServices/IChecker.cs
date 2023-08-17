using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Accounts.Dto;

namespace TransportationCompany.ApplicationServices
{
    public interface IChecker
    {
        Task<TicketDto> Check(TicketDto element);
    }
}
