using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class SearchFacilityNameQuery : FilterModel ,IRequest<Response<IEnumerable<FacilityModel>>>
    {
        public string Name { get; set; }
    }

    public class SearchNameQueryHandler : IRequestHandler<SearchFacilityNameQuery, Response<IEnumerable<FacilityModel>>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public SearchNameQueryHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<FacilityModel>>> Handle(SearchFacilityNameQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            Response<IEnumerable<FacilityModel>> result = new Response<IEnumerable<FacilityModel>>();
            var facility = await _facilityRepository.GetByNameAsync(request.Name, request.page, request.limit).ConfigureAwait(false);

            result.Data = _mapper.Map<IEnumerable<FacilityModel>>(facility);
            result.StatusCode = (int)HttpStatusCode.OK;
            result.AddPagination(await _facilityRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
            result.AddFilter(new FilterModel { page = request.page , limit = request.limit });
            return result;
        }
    }
}
