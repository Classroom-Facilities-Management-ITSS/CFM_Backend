using AutoMapper;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Schedule;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public class ScheduleMapper : Profile
	{
		public ScheduleMapper()
		{
			CreateMap<ScheduleModel, Schedule>().ReverseMap().IgnoreAllNonExisting();
			CreateMap<UpdateScheduleModel, Schedule>().ReverseMap().IgnoreAllNonExisting();
			CreateMap<AddScheduleModel, Schedule>().ReverseMap().IgnoreAllNonExisting();
		}
	}
}
