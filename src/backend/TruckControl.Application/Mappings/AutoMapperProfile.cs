using AutoMapper;
using TruckControl.Application.DTOs;
using TruckControl.Domain.Entities;

namespace TruckControl.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TruckRequestDTO, Truck>();
            CreateMap<Truck, TruckResponseDTO>();
        }
    }
}
