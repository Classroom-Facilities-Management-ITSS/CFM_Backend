using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Facility;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class FacilityMapper : Profile
    {
        public FacilityMapper()
        {
            CreateMap<FacilityModel, Facility>().ReverseMap().IgnoreAllNonExisting();
            CreateMap<AddFacilityModel, Facility>().ForMember(x => x.Classroom, opt => opt.Ignore()).ReverseMap().IgnoreAllNonExisting();
            CreateMap<UpdateFacilityModel, Facility>().ForMember(x => x.Classroom, opt => opt.Ignore()).ReverseMap().IgnoreAllNonExisting();
            CreateMap<Facility, ExportFacilityModel>()
                .ForMember(x => x.ClassroomAdrress, opt => opt.MapFrom(src => src.Classroom.Address))
                .ForMember(x => x.LastUsed, opt => opt.MapFrom(src => src.Classroom.LastUsed))
                .ReverseMap().IgnoreAllNonExisting();
        }
    }
}
