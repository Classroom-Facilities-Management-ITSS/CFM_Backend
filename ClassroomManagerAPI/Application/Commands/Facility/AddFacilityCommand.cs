using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Facility
{
    public class AddFacilityCommand : AddFacilityModel, IRequest<Response<FacilityModel>>
    {
    }

    public class AddFacilityCommandHandler : IRequestHandler<AddFacilityCommand, Response<FacilityModel>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public AddFacilityCommandHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        public async Task<Response<FacilityModel>> Handle(AddFacilityCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            Response<FacilityModel> result = new Response<FacilityModel>();
            var newFacillity = _mapper.Map<Entities.Facility>(request);
            var createdFacility = await _facilityRepository.AddAsync(newFacillity).ConfigureAwait(false);
            result.StatusCode = (int) HttpStatusCode.Created;
            _mapper.Map(createdFacility, result.Data);
            return result;
        }
    }
}
