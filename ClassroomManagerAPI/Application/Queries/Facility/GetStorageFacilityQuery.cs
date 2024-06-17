using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Facility;
using MediatR;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
    public class GetStorageFacilityQuery  : FilterModel, IRequest<ResponseMethod<IEnumerable<FacilityModel>>>
    {
    }

    public class GetStorageFacilityQueryHandler : IRequestHandler<GetStorageFacilityQuery, ResponseMethod<IEnumerable<FacilityModel>>>
    {
        private readonly IMediator _mediator;

        public GetStorageFacilityQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResponseMethod<IEnumerable<FacilityModel>>> Handle(GetStorageFacilityQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = await _mediator.Send(new SearchFacilityQuery { ClassroomAddress = Settings.StorageClass}).ConfigureAwait(false);
            return result;
        }
    }
}
