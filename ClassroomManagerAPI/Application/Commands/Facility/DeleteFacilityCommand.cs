using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
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
        public DeleteFacilityCommandHandler(IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
        }
        public async Task<ResponseMethod<string>> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<string> result = new ResponseMethod<string>();
            var deletedFacility = await _facilityRepository.DeleteAsync(request.Id).ConfigureAwait(false);
            if(!deletedFacility)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int) HttpStatusCode.NotFound;
                return result;
            }
            result.StatusCode = (int) HttpStatusCode.OK;
            result.Data = $"Delete facility with id {request.Id} sucessfully";
            return result;
        }
    }
}
