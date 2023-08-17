using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.ApplicationServices.PassengersServices;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly IPassengersAppService passengersAppService;
        public PassengersController(IPassengersAppService passengersApp)
        {
            passengersAppService = passengersApp;
        }

        [HttpGet]
        public async Task<IEnumerable<Passenger>> GetAll()
        {
            var list = await passengersAppService.GetPassengersAsync();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<Passenger> Get(int id)
        {
            var element = await passengersAppService.GetPassengersAsync(id);
            return element;
        }

        [HttpPost]
        public async void Post([FromBody] PassengersDto value)
        {
            await passengersAppService.AddPassengersAsync(value);
        }

        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] PassengersDto value)
        {
            value.Id = id;
            await passengersAppService.EditPassengersAsync(value);
        }

        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await passengersAppService.DeletePassengersAsync(id);
        }
    }
}
