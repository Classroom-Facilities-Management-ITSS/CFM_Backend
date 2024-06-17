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

    public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, ResponseMethod<string>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IClassroomRepository _classroomRepository;

        public DeleteFacilityCommandHandler(IFacilityRepository facilityRepository, IClassroomRepository classroomRepository)
        {
            _facilityRepository = facilityRepository;
            _classroomRepository = classroomRepository;
        }
        public async Task<ResponseMethod<string>> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<string> result = new ResponseMethod<string>();
            var exsiting = await _facilityRepository.GetByIDAsync(request.Id);
            if(exsiting == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }
            var classroom = await _classroomRepository.GetByIDAsync(exsiting.ClassroomId).ConfigureAwait(false);
            classroom.FacilityAmount -= exsiting.Count;
            await _classroomRepository.UpdateAsync(classroom).ConfigureAwait(false);
            await _facilityRepository.DeleteAsync(request.Id).ConfigureAwait(false);
            result.StatusCode = (int) HttpStatusCode.OK;
            result.Data = $"Delete facility with id {request.Id} sucessfully";
            return result;
        }
    }
}
