using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class GetByFacilityIdQuery : IRequest<ResponseMethod<FacilityModel>>
    {
        public Guid Id { get; set; }
    }

    public class GetByIdQueryHandler : IRequestHandler<GetByFacilityIdQuery,ResponseMethod<FacilityModel>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public GetByIdQueryHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }
        public async Task<ResponseMethod<FacilityModel>> Handle(GetByFacilityIdQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<FacilityModel> result = new ResponseMethod<FacilityModel>();
            var facility = await _facilityRepository.GetByIDAsync(request.Id);
            if(facility == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int) HttpStatusCode.NotFound;
                return result;
            }
            result.Data = _mapper.Map<FacilityModel>(facility);
            result.StatusCode = (int) HttpStatusCode.OK;
            return result;
        }
    }
}
