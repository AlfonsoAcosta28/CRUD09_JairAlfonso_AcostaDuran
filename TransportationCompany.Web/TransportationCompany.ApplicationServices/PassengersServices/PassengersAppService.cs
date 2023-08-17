using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess.Repository;

namespace TransportationCompany.ApplicationServices.PassengersServices
{
    public class PassengersAppService : IPassengersAppService
    {
        private readonly IRepository<int, Passenger> _repository;
        private readonly IMapper _mapper;
        public PassengersAppService(IRepository<int, Passenger> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PassengersDto> AddPassengersAsync(PassengersDto elementDto)
        {
            var eleent = _mapper.Map<Passenger>(elementDto);
            await _repository.AddAsync(eleent);
            return elementDto;
        }

        public async Task DeletePassengersAsync(int elementId)
        {
            await _repository.DeleteAsync(elementId);
        }

        public async Task<Passenger> EditPassengersAsync(PassengersDto elementDto)
        {
            var element = _mapper.Map<Passenger>(elementDto);
            await _repository.UpdateAsync(element);
            return element;
        }

        public async Task<Passenger> GetPassengersAsync(int elementId)
        {
            var element = await _repository.GetAsync(elementId);
            return element;
        }

        public async Task<List<Passenger>> GetPassengersAsync()
        {
            var elements = await _repository.GetAll().ToListAsync();
            return elements;

        }
    }
}
