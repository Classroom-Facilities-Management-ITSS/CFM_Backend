using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
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

        public DeleteFacilityCommandHandler(IFacilityRepository facilityRepository, IClassroomRepository classroomRepository)
        {
            _facilityRepository = facilityRepository;
            _classroomRepository = classroomRepository;
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
            await _classroomRepository.UpdateAsync(currentClassroom).ConfigureAwait(false);
            var storageClassroom = await _classroomRepository.GetByIDAsync(Configs.Settings.StorageClassId).ConfigureAwait(false);

            existingFacility.ClassroomId = Configs.Settings.StorageClassId;
            await _facilityRepository.UpdateAsync(existingFacility).ConfigureAwait(false);

            storageClassroom.FacilityAmount += existingFacility.Count;
            await _classroomRepository.UpdateAsync(storageClassroom).ConfigureAwait(false);
            
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = true;
            return result;
        }
    }
}