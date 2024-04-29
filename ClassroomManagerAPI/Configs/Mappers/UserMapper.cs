using AutoMapper;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() {
            CreateMap<AddUserRequestDto, Account>().ReverseMap();
            CreateMap<UpdatePasswordRequestDto, Account>().ReverseMap();
            CreateMap<ForgotPasswordRequestDto, Account>().ReverseMap();
        }
    }
}
