using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Facility
{
    public class UpdateFacilityCommand : UpdateFacilityModel ,IRequest<ResponseMethod<string>>
    {
    }

    public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, ResponseMethod<string>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public UpdateFacilityCommandHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }
        public async Task<ResponseMethod<string>> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<string> result = new ResponseMethod<string>();
            var facility = _mapper.Map<Entities.Facility>(request);
            var updatedFacility = await _facilityRepository.UpdateAsync(facility).ConfigureAwait(false);
            if (updatedFacility == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = $"update facility with id {request.Id} sucessfully";
            return result;
        }
    }
}
