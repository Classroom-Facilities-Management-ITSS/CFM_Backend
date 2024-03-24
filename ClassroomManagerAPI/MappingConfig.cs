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
				config.CreateMap<FacilityDto, Facility>();
				config.CreateMap<Facility, FacilityDto>();
			});
			return mappingConfig;
		} 
	}
}
