using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Schedule
{
    public class GetAllScheduleQuery : FilterModel, IRequest<ResponseMethod<IEnumerable<ScheduleModel>>>
	{
    }

	public class GetAllQueryHandler : IRequestHandler<GetAllScheduleQuery, ResponseMethod<IEnumerable<ScheduleModel>>>
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IMapper _mapper;
		public GetAllQueryHandler(IMapper mapper, IScheduleRepository scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
			_mapper = mapper;
		}
		public async Task<ResponseMethod<IEnumerable<ScheduleModel>>> Handle(GetAllScheduleQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<ScheduleModel>> result = new ResponseMethod<IEnumerable<ScheduleModel>>();
			var schedules = await _scheduleRepository.Queryable
				.Include(x => x.Classroom)
				.Include(x => x.Account)
				.ThenInclude(x => x.UserInfo)
				.Where(x => !x.IsDeleted)
                .ToListAsync().ConfigureAwait(false);
			var schedule = _scheduleRepository.GetPaginationEntity(schedules, request.page, request.limit);
			result.Data = _mapper.Map<IEnumerable<ScheduleModel>>(schedule);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(await _scheduleRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(request);
			return result;
		}
	}
}
