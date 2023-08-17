using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.ApplicationServices
{
    public class Checker : IChecker
    {

        public async Task<TicketDto> Check(TicketDto element)
        {
            using (var client = new HttpClient())
            {
                string passengerUrl = "https://localhost:7252/api/Passengers";
                string journeysUrl = "https://localhost:7252/api/Journeys";
                var response = await client.GetAsync($"{passengerUrl}/{element.PassengerId}");
                var response2 = await client.GetAsync($"{journeysUrl}/{element.JourneyId}");
                if (response.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    return element;
                }
                return null;

            }
        }
    }
}
