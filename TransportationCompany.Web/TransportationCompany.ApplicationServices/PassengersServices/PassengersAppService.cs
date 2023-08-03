using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess.Repository;

namespace TransportationCompany.ApplicationServices.PassengersServices
{
    public class PassengersAppService : IPassengersAppService
    {
        private readonly IRepository<int, Passengers> _repository;
        private readonly IMapper _mapper;
        public PassengersAppService(IRepository<int, Passengers> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PassengersDto> AddPassengersAsync(PassengersDto elementDto)
        {
            var eleent = _mapper.Map<Passengers>(elementDto);
            await _repository.AddAsync(eleent);
            return elementDto;
        }

        public async Task DeletePassengersAsync(int elementId)
        {
            await _repository.DeleteAsync(elementId);
        }

        public async Task<Passengers> EditPassengersAsync(Passengers element)
        {
            await _repository.UpdateAsync(element);

            return element;
        }

        public async Task<Passengers> GetPassengersAsync(int elementId)
        {
            var element = await _repository.GetAsync(elementId);
            return element;
        }

        public async Task<List<Passengers>> GetPassengersAsync()
        {
            var elements = await _repository.GetAll().ToListAsync();
            return elements;

        }
    }
}
