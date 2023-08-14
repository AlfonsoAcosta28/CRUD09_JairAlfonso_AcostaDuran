using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportationCompany.ApplicationServices
{
    public class Checker
    {
        private readonly HttpClient _httpClient;
        public Checker(HttpClient httpClient) { 
            _httpClient = httpClient;   
        }

        public async Task<bool> Check(int journeyId, int passengerId)
        {
            try
            {
                var journeyResponse = await _httpClient.GetAsync($"https://localhost:7252/api/Journeys/{journeyId}");
                var passengerResponse = await _httpClient.GetAsync($"https://localhost:7252/api/Passengers/{passengerId}");

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
    }
}
