﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.ApplicationServices;
using TransportationCompany.ApplicationServices.TicketsServices;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketAppService ticketAppService;
        public TicketsController(ITicketAppService ticketApp)
        {
            ticketAppService = ticketApp;
        }

        [HttpGet]
        public async Task<IEnumerable<Ticket>> GetAll()
        {
            var list = await ticketAppService.GetTicketsAsync();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<Ticket> Get(int id)
        {
            var element = await ticketAppService.GetTicketAsync(id);
            return element;
        }

        [HttpPost]
        public async void Post([FromBody] TicketDto value)
        {

            await ticketAppService.AddTicketAsync(value);
            
        }

        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] TicketDto value)
        {
            value.Id = id;
            await ticketAppService.EditTicketAsync(value);
        }

        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await ticketAppService.DeleteTicketAsync(id);
        }
    }
}
