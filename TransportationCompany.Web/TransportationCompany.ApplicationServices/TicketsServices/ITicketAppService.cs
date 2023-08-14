using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.ApplicationServices.TicketsServices
{
    public interface ITicketAppService
    {
        Task<List<Ticket>> GetTicketsAsync();

        Task<TicketDto> AddTicketAsync(TicketDto elementDto);

        Task DeleteTicketAsync(int elementId);

        Task<Ticket> GetTicketAsync(int elementId);

        Task<Ticket> EditTicketAsync(TicketDto element);
    }
}
