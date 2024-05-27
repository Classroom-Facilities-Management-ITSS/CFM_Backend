using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Schedule;

namespace ClassroomManagerAPI.Configs.Mappers
{
	public class ScheduleMapper : Profile
	{
		public ScheduleMapper()
		{
			CreateMap<ScheduleModel, Schedule>().ReverseMap();
			CreateMap<UpdateScheduleModel, Schedule>().ReverseMap();
			CreateMap<AddScheduleModel, Schedule>().ReverseMap();
		}
	}
}
