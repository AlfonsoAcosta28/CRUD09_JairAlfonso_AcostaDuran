using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.ApplicationServices.PassengersServices
{
    public interface IPassengersAppService
    {
        Task<List<Passengers>> GetPassengersAsync();

        Task<PassengersDto> AddPassengersAsync(PassengersDto elementDto);

        Task DeletePassengersAsync(int elementId);

        Task<Passengers> GetPassengersAsync(int elementId);

        Task<Passengers> EditPassengersAsync(Passengers element);
    }
}
