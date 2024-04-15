using AutoMapper;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Dto;

namespace ClassroomManagerAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<FacilityDto, Facility>().ReverseMap();
				config.CreateMap<AddFacilityRequestDto, Facility>().ReverseMap();
				config.CreateMap<ClassroomDto, Classroom>().ReverseMap();
				config.CreateMap<AddClassroomRequestDto, Classroom>().ReverseMap();
				config.CreateMap<UpdateFacilityRequestDto, Facility>().ReverseMap();
				config.CreateMap<AddUserRequestDto, Account>().ReverseMap();
				config.CreateMap<UpdatePasswordRequestDto, Account>().ReverseMap();
				config.CreateMap<ForgotPasswordRequestDto, Account>().ReverseMap();
				config.CreateMap<AccountDto, Account>().ReverseMap();
			});
			return mappingConfig;
		} 
	}
}
