using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Schedule
{
    public class DeleteScheduleCommand : IRequest<ResponseMethod<string>>
	{
		public Guid Id { get; set; }
	}

	public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, ResponseMethod<string>>
	{
		private readonly IScheduleRepository _scheduleRepository;
		public DeleteScheduleCommandHandler(IScheduleRepository scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
		}
		public async Task<ResponseMethod<string>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var deletedSchedule = await _scheduleRepository.DeleteAsync(request.Id).ConfigureAwait(false);
			if (!deletedSchedule)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"Delete schedule with id {request.Id} sucessfully";
			return result;
		}
	}
}
