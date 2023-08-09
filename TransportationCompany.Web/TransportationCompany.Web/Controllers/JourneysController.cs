using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.ApplicationServices.JourneyServices;
using TransportationCompany.ApplicationServices.JourneyServices;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.Web.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        private readonly IJourneyAppService journeyAppService;
        public JourneysController(IJourneyAppService journeyApp)
        {
            journeyAppService = journeyApp;
        }

        [HttpGet]
        public async Task<IEnumerable<Journey>> GetAll()
        {
            var list = await journeyAppService.GetJourneysAsync();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<Journey> Get(int id)
        {
            var element = await journeyAppService.GetJourneyAsync(id);
            return element;
        }

        [HttpPost]
        public async void Post([FromBody] JourneyDto value)
        {
            await journeyAppService.AddJourneyAsync(value);
        }

        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] Journey value)
        {
            value.Id = id;
            await journeyAppService.EditJourneyAsync(value);
        }

        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await journeyAppService.DeleteJourneyAsync(id);
        }
    }
}
