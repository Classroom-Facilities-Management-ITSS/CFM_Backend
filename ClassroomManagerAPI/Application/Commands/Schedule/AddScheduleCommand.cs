using AutoMapper;
using ClassroomManagerAPI.Application.Commands.Facility;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Schedule
{
	public class AddScheduleCommand : AddScheduleModel, IRequest<ResponseMethod<ScheduleModel>>
	{
	}

	public class AddScheduleCommandHandler : IRequestHandler<AddScheduleCommand, ResponseMethod<ScheduleModel>>
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IMapper _mapper;
		public AddScheduleCommandHandler(IMapper mapper, IScheduleRepository scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
			_mapper = mapper;
		}

		public async Task<ResponseMethod<ScheduleModel>> Handle(AddScheduleCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ScheduleModel> result = new ResponseMethod<ScheduleModel>();
			var newSchedule = _mapper.Map<Entities.Schedule>(request);
			var createdSchedule = await _scheduleRepository.AddAsync(newSchedule).ConfigureAwait(false);
			result.StatusCode = (int)HttpStatusCode.Created;
			result.Data = _mapper.Map<ScheduleModel>(createdSchedule);
			return result;
		}
	}
}
