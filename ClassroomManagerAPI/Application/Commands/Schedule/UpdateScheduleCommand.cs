using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Schedule
{
    public class UpdateScheduleCommand : UpdateScheduleModel, IRequest<ResponseMethod<string>>
	{
	}

	public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, ResponseMethod<string>>
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IMapper _mapper;
		public UpdateScheduleCommandHandler(IMapper mapper, IScheduleRepository scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
			_mapper = mapper;
		}
		public async Task<ResponseMethod<string>> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var schedule = _mapper.Map<Entities.Schedule>(request);
			var updatedSchedule = await _scheduleRepository.UpdateAsync(schedule).ConfigureAwait(false);
			if (updatedSchedule == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"update schedule with id {request.Id} sucessfully";
			return result;
		}
	}
}
