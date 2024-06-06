using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class GetAllFacilityQuery : FilterModel, IRequest<ResponseMethod<IEnumerable<FacilityModel>>>
    {
    }

    public class GetAllQueryHandler : IRequestHandler<GetAllFacilityQuery, ResponseMethod<IEnumerable<FacilityModel>>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public GetAllQueryHandler(IMapper mapper, IFacilityRepository facilityRepository) {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }
        public async Task<ResponseMethod<IEnumerable<FacilityModel>>> Handle(GetAllFacilityQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<IEnumerable<FacilityModel>>();
            var facilityResult = await _facilityRepository.Queryable.Include(x => x.Classroom).Where(x => !x.IsDeleted).ToListAsync().ConfigureAwait(false);
            var facilities = _facilityRepository.GetPaginationEntity(facilityResult, request.page , request.limit);
            result.Data = _mapper.Map<IEnumerable<FacilityModel>>(facilities);
            result.StatusCode = (int) HttpStatusCode.OK;
            result.AddPagination(await _facilityRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
            result.AddFilter(request);
            return result;
        }
    }
}
