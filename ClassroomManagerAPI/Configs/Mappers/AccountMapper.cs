using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Account;

namespace ClassroomManagerAPI.Configs.Mappers
{
	public class AccountMapper : Profile
	{
		public AccountMapper()
		{
			CreateMap<AccountModel, Account>().ForMember(x => x.UserInfo, opt => opt.MapFrom(src => src.User)).ReverseMap().IgnoreAllNonExisting();
			CreateMap<UpdateAccountModel, Account>().ReverseMap().IgnoreAllNonExisting();
			CreateMap<AddAccountModel, Account>().ReverseMap().IgnoreAllNonExisting();
		}
	}
}
