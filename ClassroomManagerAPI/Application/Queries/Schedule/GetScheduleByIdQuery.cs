using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Schedule
{
    public class GetScheduleByIdQuery : IRequest<ResponseMethod<ScheduleModel>>
	{
		public Guid Id { get; set; }
	}
	public class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, ResponseMethod<ScheduleModel>>
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IMapper _mapper;
		public GetScheduleByIdQueryHandler(IMapper mapper, IScheduleRepository scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
			_mapper = mapper;
		}
		public async Task<ResponseMethod<ScheduleModel>> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ScheduleModel> result = new ResponseMethod<ScheduleModel>();
			var report = await _scheduleRepository.GetByIDAsync(request.Id);
			if (report == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.Data = _mapper.Map<ScheduleModel>(report);
			result.StatusCode = (int)HttpStatusCode.OK;
			return result;
		}
	}
}
