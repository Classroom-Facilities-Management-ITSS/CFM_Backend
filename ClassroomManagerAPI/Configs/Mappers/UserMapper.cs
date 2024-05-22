using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.User;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() {
            CreateMap<UserModel, UserInfo>().ReverseMap();
        }
    }
}
