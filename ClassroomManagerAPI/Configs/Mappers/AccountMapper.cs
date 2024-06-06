using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Account;

namespace ClassroomManagerAPI.Configs.Mappers
{
	public class AccountMapper : Profile
	{
		public AccountMapper()
		{
			CreateMap<Account, AccountModel>().ForMember(x => x.User, opt => opt.MapFrom(src => src.UserInfo)).ReverseMap().IgnoreAllNonExisting();
			CreateMap<UpdateAccountModel, Account>().ReverseMap().IgnoreAllNonExisting();
			CreateMap<AddAccountModel, Account>().ReverseMap().IgnoreAllNonExisting();
		}
	}
}
