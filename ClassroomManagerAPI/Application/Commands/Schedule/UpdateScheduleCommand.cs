using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Schedule
{
	public class UpdateScheduleCommand : UpdateScheduleModel, IRequest<ResponseMethod<ScheduleModel>>
	{
	}

	public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, ResponseMethod<ScheduleModel>>
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IClassroomRepository _classroomRepository;
		private readonly IMapper _mapper;

		public UpdateScheduleCommandHandler(IMapper mapper, IScheduleRepository scheduleRepository, IAccountRepository accountRepository, IClassroomRepository classroomRepository)
		{
			_scheduleRepository = scheduleRepository;
			_accountRepository = accountRepository;
			_classroomRepository = classroomRepository;
			_mapper = mapper;
		}

		public async Task<ResponseMethod<ScheduleModel>> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ScheduleModel> result = new ResponseMethod<ScheduleModel>();

			var account = await _accountRepository.GetByIDAsync(request.AccountId.Value);
			if (account == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}

			var classroom = await _classroomRepository.GetByIDAsync(request.ClassroomId.Value);
			if (classroom == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}

			var schedule = _mapper.Map<Entities.Schedule>(request);
			var updatedSchedule = await _scheduleRepository.UpdateAsync(schedule).ConfigureAwait(false);
			if (updatedSchedule == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}

			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = _mapper.Map<ScheduleModel>(updatedSchedule);
			return result;
		}
	}
}
