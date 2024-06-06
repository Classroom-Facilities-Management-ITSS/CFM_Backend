using AutoMapper;
using ClassroomManagerAPI.Application.Commands.Facility;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
	public class DeleteClassroomCommand : IRequest<ResponseMethod<bool>>
	{
		public Guid Id { get; set; }
	}
	public class DeleteClassroomCommandHandler : IRequestHandler<DeleteClassroomCommand, ResponseMethod<bool>>
	{
		private readonly IClassroomRepository _classroomRepository;
		private readonly IFacilityRepository _facilityRepository;
		private readonly Guid _storageClassroomId;
		private readonly IMediator _mediator;

		public DeleteClassroomCommandHandler(IClassroomRepository classroomRepository, IMediator mediator, IFacilityRepository facilityRepository)
		{
			_classroomRepository = classroomRepository;
			_mediator = mediator;
			_facilityRepository = facilityRepository;
		}

		public async Task<ResponseMethod<bool>> Handle(DeleteClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<bool> result = new ResponseMethod<bool>();
			var classroom = await _classroomRepository.Queryable
				.Include(x => x.Facilities)
				.Include(x => x.Schedules)
				.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
			if (classroom == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				result.Data = false;
				return result;
			}
			if (classroom.Address.Equals(Configs.Settings.StorageClass))
			{
				result.AddBadRequest(nameof(ErrorClassEnum.DeleteStorageError));
				result.StatusCode = StatusCodes.Status500InternalServerError;
				result.Data = false;
				return result;
			}
			var hasConflicts = classroom.FacilityAmount != 0 || classroom.Schedules.Any(f => f.EndTime <= DateTime.Now.AddDays(7));
			//var classroom = _classroomRepository.Queryable().Include(x => x.Schedules).Any(x => (!x.IsDeleted) && (x.FacilityAmount != 0 || x.Schedules.Any(f => f.EndTime <= DateTime.Now.AddDays(7))));
			if (hasConflicts)
			{
				result.AddBadRequest(nameof(ErrorClassEnum.ConflictSchedule));
				result.StatusCode = (int)HttpStatusCode.Conflict;
				result.Data = false;
				return result;
			}
			var storageClassroom = await _classroomRepository.Queryable.FirstOrDefaultAsync(x => x.Address.Equals(Configs.Settings.StorageClass)).ConfigureAwait(false);
			foreach (var facility in classroom.Facilities)
			{
				facility.ClassroomId = _storageClassroomId;
				//await _facilityRepository.UpdateAsync(facility).ConfigureAwait(false);
				await _mediator.Send(new UpdateFacilityCommand { ClassroomId = facility.ClassroomId }).ConfigureAwait(false);
			}
			storageClassroom.FacilityAmount += classroom.FacilityAmount;
			classroom.FacilityAmount = 0;
			//await _classroomRepository.UpdateAsync(storageClassroom).ConfigureAwait(false);
			await _mediator.Send(new UpdateClassroomCommand { Address = storageClassroom.Address }).ConfigureAwait(false);
			// await _classroomRepository.UpdateAsync(classroom).ConfigureAwait(false);
			await _mediator.Send(new UpdateClassroomCommand { Address = classroom.Address }).ConfigureAwait(false);
			var deletedClassrooom = await _classroomRepository.DeleteAsync(request.Id).ConfigureAwait(false);
			if (!deletedClassrooom)
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
