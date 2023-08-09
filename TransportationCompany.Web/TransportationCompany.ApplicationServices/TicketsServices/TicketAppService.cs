using AutoMapper;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
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
        public TicketAppService(IRepository<int, Ticket> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TicketDto> AddTicketAsync(TicketDto elementDto)
        {
            var element = _mapper.Map<Ticket>(elementDto);
            await _repository.AddAsync(element);
            return elementDto;
        }

        public async Task DeleteTicketAsync(int elementId)
        {
            await _repository.DeleteAsync(elementId);
        }

        public async Task<Ticket> EditTicketAsync(Ticket element)
        {
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
