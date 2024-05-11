using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Account;

namespace ClassroomManagerAPI.Configs.Mappers
{
	public class AccountMapper : Profile
	{
		public AccountMapper()
		{
			CreateMap<AccountModel, Account>().ReverseMap();
			CreateMap<UpdateAccountModel, Account>().ReverseMap();
			CreateMap<AddAccountModel, Account>().ReverseMap();
		}
	}
}
