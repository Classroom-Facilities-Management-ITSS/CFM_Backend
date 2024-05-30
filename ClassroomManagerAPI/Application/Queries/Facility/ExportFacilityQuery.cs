using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Helpers;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class ExportFacilityQuery : SearchFacilityModel, IRequest<ResponseMethod<Stream>>
    {
    }

    public class ExportFacilityQueryHandler : IRequestHandler<ExportFacilityQuery, ResponseMethod<Stream>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;

        public ExportFacilityQueryHandler(IFacilityRepository facilityRepository, IMapper mapper)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }
        public async Task<ResponseMethod<Stream>> Handle(ExportFacilityQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<Stream>();
            var facility = _facilityRepository.Queryable().Include(x => x.Classroom).AsQueryable();

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

            var facilityExport = _mapper.Map<IList<ExportFacilityModel>>(facility);
            var stream = facilityExport.ExportExcel();
            result.Data = stream;
            result.StatusCode = StatusCodes.Status200OK;
            return result;
        }
    }
}
