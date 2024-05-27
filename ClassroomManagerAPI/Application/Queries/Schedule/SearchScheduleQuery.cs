using AutoMapper;
using ClassroomManagerAPI.Application.Queries.Facility;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Schedule
{
	public class SearchScheduleQuery : FilterModel, IRequest<ResponseMethod<IEnumerable<ScheduleModel>>>
	{
	}

	public class SearchScheduleQueryHandler : IRequestHandler<SearchScheduleQuery, ResponseMethod<IEnumerable<ScheduleModel>>>
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IMapper _mapper;
		public SearchScheduleQueryHandler(IMapper mapper, IScheduleRepository scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
			_mapper = mapper;
		}

		public async Task<ResponseMethod<IEnumerable<ScheduleModel>>> Handle(SearchScheduleQuery request, CancellationToken cancellationToken)
		{
			// Write ScheduleSearch here
		}
	}
}
