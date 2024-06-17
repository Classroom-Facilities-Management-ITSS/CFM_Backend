using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Schedule
{
    public class DeleteScheduleCommand : IRequest<ResponseMethod<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, ResponseMethod<bool>>
    {
        private readonly IScheduleRepository _scheduleRepository;
        public DeleteScheduleCommandHandler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        public async Task<ResponseMethod<bool>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<bool> result = new ResponseMethod<bool>();
            var deletedSchedule = await _scheduleRepository.DeleteAsync(request.Id).ConfigureAwait(false);
            if (!deletedSchedule)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                result.Data = false;
                return result;
            }
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = true;
            return result;
        }
    }
}