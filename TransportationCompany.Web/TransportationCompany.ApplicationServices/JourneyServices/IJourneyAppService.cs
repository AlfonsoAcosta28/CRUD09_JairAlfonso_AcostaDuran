using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.ApplicationServices.JourneyServices
{
    public interface IJourneyAppService
    {
        Task<List<Journey>> GetJourneysAsync();

        Task<JourneyDto> AddJourneyAsync(JourneyDto elementDto);

        Task DeleteJourneyAsync(int elementId);

        Task<Journey> GetJourneyAsync(int elementId);

        Task<Journey> EditJourneyAsync(JourneyDto element);
    }
}
