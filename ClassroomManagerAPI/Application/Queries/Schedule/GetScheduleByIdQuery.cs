using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
			var schedule = await _scheduleRepository.Queryable
													.Include(x => x.Classroom)
													.Include(x => x.Account)
													.ThenInclude(x => x.UserInfo)
													.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == request.Id)
													.ConfigureAwait(false);

            if (schedule == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.Data = _mapper.Map<ScheduleModel>(schedule);
			result.StatusCode = (int)HttpStatusCode.OK;
			return result;
		}
	}
}
