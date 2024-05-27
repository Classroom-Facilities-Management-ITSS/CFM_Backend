using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using MediatR;
using ClassroomManagerAPI.Models.Schedule;
using AutoMapper;
using ClassroomManagerAPI.Application.Queries.Report;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories.IRepositories;
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
			var report = await _scheduleRepository.GetAllAsync(request.page, request.limit).ConfigureAwait(false);
			result.Data = _mapper.Map<IEnumerable<ScheduleModel>>(report);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(await _scheduleRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(request);
			return result;
		}
	}
}
