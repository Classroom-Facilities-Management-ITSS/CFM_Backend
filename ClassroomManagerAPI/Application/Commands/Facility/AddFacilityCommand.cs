using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Facility
{
    public class AddFacilityCommand : AddFacilityModel, IRequest<ResponseMethod<FacilityModel>>
    {
    }

    public class AddFacilityCommandHandler : IRequestHandler<AddFacilityCommand, ResponseMethod<FacilityModel>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public AddFacilityCommandHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMethod<FacilityModel>> Handle(AddFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<FacilityModel> result = new ResponseMethod<FacilityModel>();
            var newFacillity = _mapper.Map<Entities.Facility>(request);
            var createdFacility = await _facilityRepository.AddAsync(newFacillity).ConfigureAwait(false);
            result.StatusCode = (int) HttpStatusCode.Created;
            result.Data = _mapper.Map<FacilityModel>(createdFacility);
            return result;
        }
    }
}
