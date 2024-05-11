using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class GetByFacilityIdQuery : IRequest<Response<FacilityModel>>
    {
        public Guid Id { get; set; }
    }

    public class GetByIdQueryHandler : IRequestHandler<GetByFacilityIdQuery,Response<FacilityModel>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public GetByIdQueryHandler(IMapper mapper, IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }
        public async Task<Response<FacilityModel>> Handle(GetByFacilityIdQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            Response<FacilityModel> result = new Response<FacilityModel>();
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
