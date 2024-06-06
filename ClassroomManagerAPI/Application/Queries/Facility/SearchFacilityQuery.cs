using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class SearchFacilityQuery : SearchFacilityModel ,IRequest<ResponseMethod<IEnumerable<FacilityModel>>>
    {

	}

    public class SearchNameQueryHandler : IRequestHandler<SearchFacilityQuery, ResponseMethod<IEnumerable<FacilityModel>>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public SearchNameQueryHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMethod<IEnumerable<FacilityModel>>> Handle(SearchFacilityQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<IEnumerable<FacilityModel>> result = new ResponseMethod<IEnumerable<FacilityModel>>();
            var facility = _facilityRepository.Queryable.Include(x => x.Classroom).AsQueryable();

            if (request.Name != null)
            {
                facility = facility.Where(x => !x.IsDeleted && x.Name.ToLower().Trim().Contains(request.Name.ToLower().Trim()));
            }
			if (request.Count != null)
			{
				facility = facility.Where(x => !x.IsDeleted && x.Count >= request.Count);
			}
			if (request.Version != null)
			{
				facility = facility.Where(x => !x.IsDeleted && x.Version.ToLower().Trim().Contains(request.Version.ToLower().Trim()));
			}
            if (request.ClassroomAddress != null)
            {
                facility = facility.Where(x => !x.IsDeleted && x.Classroom.Address.ToLower().Trim().Contains(request.ClassroomAddress.ToLower().Trim()));
            }
            var facilityResult = _facilityRepository.GetPaginationEntity(facility, request.page, request.limit);

			result.Data = _mapper.Map<IEnumerable<FacilityModel>>(facilityResult);
            result.StatusCode = (int)HttpStatusCode.OK;
            result.AddPagination(_facilityRepository.PaginationEntity(facility,request.page, request.limit));
            result.AddFilter(request);
            return result;
        }
    }
}
