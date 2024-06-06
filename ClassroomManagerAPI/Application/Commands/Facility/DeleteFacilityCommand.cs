using ClassroomManagerAPI.Application.Commands.Classroom;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Facility
{
	public class DeleteFacilityCommand : IRequest<ResponseMethod<bool>>
	{
		public Guid Id { get; set; }
	}

	public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, ResponseMethod<bool>>
	{
		private readonly IFacilityRepository _facilityRepository;
		private readonly IClassroomRepository _classroomRepository;
		private readonly IMediator _mediator;

		public DeleteFacilityCommandHandler(IFacilityRepository facilityRepository, IClassroomRepository classroomRepository, IMediator mediator)
		{
			_facilityRepository = facilityRepository;
			_classroomRepository = classroomRepository;
			_mediator = mediator;
		}
		public async Task<ResponseMethod<bool>> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<bool> result = new ResponseMethod<bool>();
			var existingFacility = await _facilityRepository.GetByIDAsync(request.Id);
			if (existingFacility == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				result.Data = false;
				return result;
			}
			var currentClassroom = await _classroomRepository.GetByIDAsync(existingFacility.ClassroomId).ConfigureAwait(false);
			currentClassroom.FacilityAmount -= existingFacility.Count;
			// await _classroomRepository.UpdateAsync(currentClassroom).ConfigureAwait(false);
			var storageClassroom = await _classroomRepository.Queryable.FirstOrDefaultAsync(x => x.Address.Equals(Configs.Settings.StorageClass)).ConfigureAwait(false);
			existingFacility.ClassroomId = storageClassroom.Id;
			// await _facilityRepository.UpdateAsync(existingFacility).ConfigureAwait(false);
			await _mediator.Send(new UpdateFacilityCommand { Id = existingFacility.Id, ClassroomId = existingFacility.ClassroomId }).ConfigureAwait(false);
			storageClassroom.FacilityAmount += existingFacility.Count;
			// await _classroomRepository.UpdateAsync(storageClassroom).ConfigureAwait(false);
			await _mediator.Send(new UpdateClassroomCommand { Address = storageClassroom.Address }).ConfigureAwait(false);

			await _facilityRepository.DeleteAsync(request.Id).ConfigureAwait(false);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = true;
			return result;
		}
	}
}
