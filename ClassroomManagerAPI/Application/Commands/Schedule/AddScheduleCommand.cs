using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
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
        private readonly IAccountRepository _accountRepository;
        private readonly IClassroomRepository _classroomRepository;
        private readonly IMapper _mapper;

        public AddScheduleCommandHandler(IMapper mapper, IScheduleRepository scheduleRepository, IAccountRepository accountRepository, IClassroomRepository classroomRepository)
        {
            _scheduleRepository = scheduleRepository;
            _accountRepository = accountRepository;
            _classroomRepository = classroomRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMethod<ScheduleModel>> Handle(AddScheduleCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<ScheduleModel> result = new ResponseMethod<ScheduleModel>();

            // Validate AccountId
            var account = await _accountRepository.GetByIDAsync(request.AccountId.Value);
            if (account == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }

            // Validate ClassroomId
            var classroom = await _classroomRepository.GetByIDAsync(request.ClassroomId.Value);
            if (classroom == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }

            // Create new schedule
            var newSchedule = _mapper.Map<Entities.Schedule>(request);
            var createdSchedule = await _scheduleRepository.AddAsync(newSchedule).ConfigureAwait(false);

            result.StatusCode = (int)HttpStatusCode.Created;
            result.Data = _mapper.Map<ScheduleModel>(createdSchedule);
            return result;
        }
    }
}