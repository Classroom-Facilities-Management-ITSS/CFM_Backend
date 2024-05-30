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
            CreateMap<Facility, ExportFacilityModel>()
                .ForMember(x => x.ClassroomAdrress, opt => opt.MapFrom(src => src.Classroom.Address))
                .ForMember(x => x.LastUsed, opt => opt.MapFrom(src => src.Classroom.LastUsed))
                .ReverseMap();
        }
    }
}
