using AutoMapper;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class FacilityMapper : Profile
    {
        public FacilityMapper()
        {
            CreateMap<FacilityDto, Facility>().ReverseMap();
            CreateMap<AddFacilityRequestDto, Facility>().ReverseMap();
            CreateMap<UpdateFacilityRequestDto, Facility>().ReverseMap();
        }
    }
}
