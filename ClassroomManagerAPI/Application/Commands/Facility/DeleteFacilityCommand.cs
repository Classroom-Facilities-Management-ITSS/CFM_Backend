using ClassroomManagerAPI.Application.Commands.Classroom;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Facility
{
    public class DeleteFacilityCommand : IRequest<ResponseMethod<string>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, ResponseMethod<string>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IClassroomRepository _classroomRepository;
		private readonly IMediator _mediator;
		private readonly Guid _storageClassroomId;

        public DeleteFacilityCommandHandler(IFacilityRepository facilityRepository, IClassroomRepository classroomRepository, IMediator mediator)
        {
            _facilityRepository = facilityRepository;
            _classroomRepository = classroomRepository;
			_mediator = mediator;
			_storageClassroomId = Guid.Parse("0540dec7-c15c-4d3d-9b24-a1cfca346209");
        }
        public async Task<ResponseMethod<string>> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<string> result = new ResponseMethod<string>();
            var existingFacility = await _facilityRepository.GetByIDAsync(request.Id);
            if(existingFacility == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }
            var currentClassroom = await _classroomRepository.GetByIDAsync(existingFacility.ClassroomId).ConfigureAwait(false);
			currentClassroom.FacilityAmount -= existingFacility.Count;
            //await _classroomRepository.UpdateAsync(currentClassroom).ConfigureAwait(false);
            await _mediator.Send(new UpdateClassroomCommand { Address = currentClassroom.Address }).ConfigureAwait(false);
            var storageClassroom = await _classroomRepository.GetByIDAsync(_storageClassroomId).ConfigureAwait(false);
            if(storageClassroom == null)
            {
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
            existingFacility.ClassroomId = _storageClassroomId;
            // await _facilityRepository.UpdateAsync(existingFacility).ConfigureAwait(false);
            await _mediator.Send(new UpdateFacilityCommand { ClassroomId = existingFacility.ClassroomId }).ConfigureAwait(false);
            storageClassroom.FacilityAmount += existingFacility.Count;
            // await _classroomRepository.UpdateAsync(storageClassroom).ConfigureAwait(false);
            await _mediator.Send(new UpdateClassroomCommand { Address = storageClassroom.Address }).ConfigureAwait(false);

            await _facilityRepository.DeleteAsync(request.Id).ConfigureAwait(false);
            result.StatusCode = (int) HttpStatusCode.OK;
            result.Data = $"Delete facility with id {request.Id} sucessfully";
            return result;
        }
    }
}
