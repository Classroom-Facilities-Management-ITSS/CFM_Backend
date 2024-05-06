using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class GetAllFacilityQuery : FilterModel, IRequest<Response<IEnumerable<FacilityModel>>>
    {
    }

    public class GetAllQueryHandler : IRequestHandler<GetAllFacilityQuery, Response<IEnumerable<FacilityModel>>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public GetAllQueryHandler(IMapper mapper, IFacilityRepository facilityRepository) {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<FacilityModel>>> Handle(GetAllFacilityQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            Response<IEnumerable<FacilityModel>> result = new Response<IEnumerable<FacilityModel>>();
            var facilityResult = await _facilityRepository.GetAllAsync(request.page, request.limit).ConfigureAwait(false);
            result.Data = _mapper.Map<IEnumerable<FacilityModel>>(facilityResult);
            result.StatusCode = (int) HttpStatusCode.OK;
            result.AddPagination(await _facilityRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
            result.AddFilter(request);
            return result;
        }
    }
}
