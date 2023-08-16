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
        //private readonly HttpClient _httpClient;

        //private readonly IHttpClientFactory _httpClientFactory;

        public Checker()
        {
            //_httpClient = httpClient;
            //_httpClientFactory = httpClientFactory;
        }

        public async Task<bool> Check(TicketDto element)
        {
            using (var client = new HttpClient())
            {
                string passengerUrl = "https://localhost:7252/api/Passengers";
                string journeysUrl = "https://localhost:7252/api/Journeys";
                var response = await client.GetAsync($"{passengerUrl}/{element.PassengerId}");
                var response2 = await client.GetAsync($"{journeysUrl}/{element.JourneyId}");
                if (response.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    // var content = await response.Content.ReadAsStringAsync();
                    // Passenger passengerJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Passenger>(content);


                    return true;
                }
                return false;

            }
            /*
            using (var client = new HttpClient())
            {
                try
                {
                    var journeyResponse = await client.GetAsync($"https://localhost:7252/api/Journeys/{element.JourneyId}");
                    var passengerResponse = await client.GetAsync($"https://localhost:7252/api/Passengers/{element.PassengerId}");

                    if (!journeyResponse.IsSuccessStatusCode || !passengerResponse.IsSuccessStatusCode)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al verificar: {ex.Message}");
                    return false;
                }
            }
            */
        }
    }
}
