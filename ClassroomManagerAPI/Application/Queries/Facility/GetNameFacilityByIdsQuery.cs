using ClassroomManagerAPI.Common;
using MediatR;

namespace ClassroomManagerAPI.Application.Queries.Facility
{
	public class GetNameFacilityByIdsQuery : IRequest<ResponseMethod<IEnumerable<string>>>
	{
		public IList<Guid>? ReportFacilites { get; set; }
	}

	public class GetNameFacilityByIdsQueryHandler : IRequestHandler<GetNameFacilityByIdsQuery, ResponseMethod<IEnumerable<string>>>
	{
		private readonly IMediator _mediator;

		public GetNameFacilityByIdsQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ResponseMethod<IEnumerable<string>>> Handle(GetNameFacilityByIdsQuery request, CancellationToken cancellationToken)
		{
			 ArgumentNullException.ThrowIfNull(request);
			var methodResponse = new ResponseMethod<IEnumerable<string>>();
			var listFacility = new List<string>();
			foreach(var report in request.ReportFacilites)
			{
				var result = await _mediator.Send(new GetByFacilityIdQuery { Id = report }).ConfigureAwait(false);
				listFacility.Add(result.Data.Name);
			}

			methodResponse.Data = listFacility;
			methodResponse.StatusCode = StatusCodes.Status200OK;
			return methodResponse;
		}
	}
}
