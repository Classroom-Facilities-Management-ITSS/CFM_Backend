using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Facility;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class FacilityMapper : Profile
    {
        public FacilityMapper()
        {
            CreateMap<FacilityModel, Facility>().ReverseMap();
            CreateMap<AddFacilityModel, Facility>().ReverseMap();
            CreateMap<UpdateFacilityModel, Facility>().ReverseMap();
        }
    }
}
