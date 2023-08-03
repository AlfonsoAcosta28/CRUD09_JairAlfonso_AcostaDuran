using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess.Repository;

namespace TransportationCompany.ApplicationServices.JourneyServices
{
    public class JourneyAppService : IJourneyAppService
    {
        private readonly IRepository<int, Journey> _repository;
        private readonly IMapper _mapper;
        public JourneyAppService(IRepository<int, Journey> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<JourneyDto> AddJourneyAsync(JourneyDto elementDto)
        {
            var eleent = _mapper.Map<Journey>(elementDto);
            await _repository.AddAsync(eleent);
            return elementDto;
        }

        public async Task DeleteJourneyAsync(int elementId)
        {
            await _repository.DeleteAsync(elementId);
        }

        public async Task<Journey> EditJourneyAsync(Journey element)
        {
            await _repository.UpdateAsync(element);

            return element;
        }

        public async Task<Journey> GetJourneyAsync(int elementId)
        {
            var element = await _repository.GetAsync(elementId);
            return element;
        }

        public async Task<List<Journey>> GetJourneysAsync()
        {
            var elements = await _repository.GetAll().ToListAsync();
            return elements;

        }
    }
}
