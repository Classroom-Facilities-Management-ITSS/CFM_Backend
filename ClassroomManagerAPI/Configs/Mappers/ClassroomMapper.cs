using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Classroom;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class ClassroomMapper : Profile
    {
        public ClassroomMapper() {
            CreateMap<ClassroomModel, Classroom>().ReverseMap().IgnoreAllNonExisting();
            CreateMap<AddClassroomModel, Classroom>().ForMember(x => x.Facilities, opt => opt.Ignore()).ReverseMap().IgnoreAllNonExisting();
            CreateMap<UpdateClassroomModel, Classroom>().ForMember(x => x.Facilities, opt => opt.Ignore()).ReverseMap().IgnoreAllNonExisting();
        }
    }
}
