using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class SearchFacilityNameQuery : IRequest<Response<FacilityModel>>
    {
        public string Name { get; set; }
    }

    public class SearchNameQueryHandler : IRequestHandler<SearchFacilityNameQuery, Response<FacilityModel>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public SearchNameQueryHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        public async Task<Response<FacilityModel>> Handle(SearchFacilityNameQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            Response<FacilityModel> result = new Response<FacilityModel>();
            var facility = await _facilityRepository.GetByNameAsync(request.Name).ConfigureAwait(false);
            if (facility == null)
            {
                result.AddBadRequest($"Facility with name {request.Name} not existing");
                result.StatusCode = (int)HttpStatusCode.NotFound;
                return result;
            }
            _mapper.Map(facility, result.Data);
            result.StatusCode = (int)HttpStatusCode.OK;
            return result;
        }
    }
}
