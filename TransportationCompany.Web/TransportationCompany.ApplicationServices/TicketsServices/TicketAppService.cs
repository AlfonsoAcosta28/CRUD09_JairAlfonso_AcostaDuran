using AutoMapper;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess.Repository;

namespace TransportationCompany.ApplicationServices.TicketsServices
{
    public class TicketAppService : ITicketAppService
    {
        private readonly IRepository<int, Ticket> _repository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        public TicketAppService(IRepository<int, Ticket> repository, IMapper mapper
            , IHttpClientFactory httpClient)
            //)
        {
            _repository = repository;
            _httpClientFactory = httpClient;
            _mapper = mapper;
        }
        public async Task<TicketDto> AddTicketAsync(TicketDto elementDto)
        {
           
            var httpClient = _httpClientFactory.CreateClient();
            var checker = new Checker(httpClient);

            if (await checker.Check(elementDto.JourneyId, elementDto.PassengerId))
            {
                var element = _mapper.Map<Ticket>(elementDto);
                await _repository.AddAsync(element);
                return elementDto;
            }
            else
            {
                throw new InvalidOperationException("JourneyId y/o PassengerId no existen en otros microservicios.");
            }
        }


        public async Task DeleteTicketAsync(int elementId)
        {
            await _repository.DeleteAsync(elementId);
        }

        public async Task<Ticket> EditTicketAsync(TicketDto elementDto)
        {
            var element = _mapper.Map<Ticket>(elementDto);
            await _repository.UpdateAsync(element);
            return element;
        }

        public async Task<Ticket> GetTicketAsync(int elementId)
        {
            var element = await _repository.GetAsync(elementId);
            return element;
        }

        public async Task<List<Ticket>> GetTicketsAsync()
        {
            var elements = await _repository.GetAll().ToListAsync();
            return elements;

        }
    }
}
