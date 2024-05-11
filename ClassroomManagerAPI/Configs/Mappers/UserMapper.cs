using AutoMapper;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.User;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() {
            CreateMap<UserModel, UserInfo>().ReverseMap();
            CreateMap<AddUserRequestDto, Account>().ReverseMap();
            CreateMap<UpdatePasswordRequestDto, Account>().ReverseMap();
            CreateMap<ForgotPasswordRequestDto, Account>().ReverseMap();
        }
    }
}
