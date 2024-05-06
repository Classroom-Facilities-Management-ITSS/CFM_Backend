using AutoMapper;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Classroom;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class ClassroomMapper : Profile
    {
        public ClassroomMapper() {
            CreateMap<ClassroomModel, Classroom>().ReverseMap();
            CreateMap<AddClassroomModel, Classroom>().ReverseMap();
            CreateMap<UpdateClassroomModel, Classroom>().ReverseMap();
        }
    }
}
