using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Account;

namespace ClassroomManagerAPI.Configs.Mappers
{
	public class AccountMapper : Profile
	{
		public AccountMapper()
		{
			CreateMap<AccountModel, Account>().ForMember(x => x.UserInfo, opt => opt.MapFrom(src => src.User)).ReverseMap();
			CreateMap<UpdateAccountModel, Account>().ReverseMap();
			CreateMap<AddAccountModel, Account>().ReverseMap();
		}
	}
}
