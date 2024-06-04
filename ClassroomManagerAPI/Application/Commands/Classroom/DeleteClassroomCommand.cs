using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
    public class DeleteClassroomCommand : IRequest<ResponseMethod<string>>
	{
		public Guid Id { get; set; }
	}
	public class DeleteClassroomCommandHandler : IRequestHandler<DeleteClassroomCommand, ResponseMethod<string>>
	{
		private readonly IClassroomRepository _classroomRepository;

		public DeleteClassroomCommandHandler(IClassroomRepository classroomRepository)
        {
			_classroomRepository = classroomRepository;
		}

		public async Task<ResponseMethod<string>> Handle(DeleteClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var classroom = _classroomRepository.Queryable.Include(x => x.Schedules).Any(x => (!x.IsDeleted) && (x.FacilityAmount != 0 || x.Schedules.Any(f => f.EndTime <= DateTime.Now.AddDays(7))));
			if (classroom)
			{
				result.AddBadRequest(nameof(ErrorClassEnum.ConflictSchedule));
				result.StatusCode = (int)HttpStatusCode.Conflict;
				return result;
			}
			var deletedClassrooom = await _classroomRepository.DeleteAsync(request.Id).ConfigureAwait(false);
			if (!deletedClassrooom)
			{
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"Delete classroom with id {request.Id} sucessfully";
			return result;
		}
	}
}
