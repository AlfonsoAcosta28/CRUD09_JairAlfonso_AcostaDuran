using AutoMapper;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess;
using TransportationCompany.DataAccess.Repository;

namespace TransportationCompany.ApplicationServices.TicketsServices
{
    public class TicketAppService : ITicketAppService
    {
        private readonly IRepository<int, Ticket> _repository;
        private readonly IMapper _mapper;
        private readonly IChecker _checker;
        public TicketAppService(IRepository<int, Ticket> repository, IMapper mapper, IChecker checker)
        {
            _repository = repository;
            _mapper = mapper;
            _checker = checker;
        }
        public async Task<TicketDto> AddTicketAsync(TicketDto elementDto)
        {
            
            //if (await _checker.Check(elementDto) != null)
            {
               // var a = _repository.Context;
               var element = _mapper.Map<Ticket>(elementDto);
              //  using (var repository = IRepository())
                {
              //      var c = _repository.Context;
                    await _repository.AddAsync(element);
                    return elementDto;
                }
            }
            //else
            {
              //  throw new InvalidOperationException("JourneyId y/o PassengerId no existen en otros microservicios.");
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
