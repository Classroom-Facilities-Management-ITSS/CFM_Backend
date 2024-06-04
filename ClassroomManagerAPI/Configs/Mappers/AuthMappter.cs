using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Auth;

namespace ClassroomManagerAPI.Configs.Mappers
{
	public class AuthMapper : Profile
	{
		public AuthMapper()
		{
			CreateMap<RegisterModel, Account>().ReverseMap().IgnoreAllNonExisting();
		}
	}
}
