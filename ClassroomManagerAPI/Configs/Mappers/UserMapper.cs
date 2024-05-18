using AutoMapper;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.User;
using ClassroomManagerAPI.Models.Auth;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() {
            CreateMap<UserModel, UserInfo>().ReverseMap();
        }
    }
}
