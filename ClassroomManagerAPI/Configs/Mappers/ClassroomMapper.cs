using AutoMapper;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class ClassroomMapper : Profile
    {
        public ClassroomMapper() {
            CreateMap<ClassroomDto, Classroom>().ReverseMap();
            CreateMap<AddClassroomRequestDto, Classroom>().ReverseMap();
        }
    }
}
