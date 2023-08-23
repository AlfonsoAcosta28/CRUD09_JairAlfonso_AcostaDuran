using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Passenger> passengers { get; set; }
        public List<Ticket> tickets { get; set; }
        public List<Journey> journeys { get; set; }
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            passengers = new List<Passenger>(); 
            tickets = new List<Ticket>();
            journeys = new List<Journey>();
        }

        public async Task OnGetAsync()
        {
            System.Threading.Thread.Sleep(5000);
            HttpClient httpClient = new HttpClient();
            var responsePassenger = await httpClient.GetAsync("https://localhost:7252/api/Passengers");
            var responseTicket = await httpClient.GetAsync("https://localhost:7252/api/Tickets");
            var responseJourneys = await httpClient.GetAsync("https://localhost:7252/api/Journeys");

            if (responsePassenger.IsSuccessStatusCode)
            {
                var content = await responsePassenger.Content.ReadAsStringAsync();
                passengers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Passenger>>(content);
            }

            if (responseTicket.IsSuccessStatusCode)
            {
                var content = await responseTicket.Content.ReadAsStringAsync();
                tickets = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ticket>>(content);
            }

            if (responseJourneys.IsSuccessStatusCode)
            {
                var content = await responseJourneys.Content.ReadAsStringAsync();
                journeys = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Journey>>(content);
            }
        }
    }
}