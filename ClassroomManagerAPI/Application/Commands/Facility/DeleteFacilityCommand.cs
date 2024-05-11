using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Facility
{
    public class DeleteFacilityCommand : IRequest<Response<string>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, Response<string>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public DeleteFacilityCommandHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }
        public async Task<Response<string>> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            Response<string> result = new Response<string>();
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
