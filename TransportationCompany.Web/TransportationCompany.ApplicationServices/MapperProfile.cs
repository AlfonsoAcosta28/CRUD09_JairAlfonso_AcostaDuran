using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.Core.Entities;

namespace TransportationCompany.ApplicationServices
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {

            CreateMap<Journey, JourneyDto>();
            CreateMap<JourneyDto, Journey>();

            CreateMap<Passengers, PassengersDto>();
            CreateMap<PassengersDto, Passengers>();

            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();
        }
    }
}
