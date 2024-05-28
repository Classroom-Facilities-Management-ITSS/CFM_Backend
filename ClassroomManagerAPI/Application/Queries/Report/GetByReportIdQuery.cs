using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Report
{
    public class GetByReportIdQuery : IRequest<ResponseMethod<ReportModel>>
	{
        public Guid Id { get; set; }
    }

	public class GetByReportIdQueryHandler : IRequestHandler<GetByReportIdQuery, ResponseMethod<ReportModel>>
	{
		private readonly IReportRepository _reportRepository;
		private readonly IMapper _mapper;
		public GetByReportIdQueryHandler(IMapper mapper, IReportRepository reportRepository)
		{
			_reportRepository = reportRepository;
			_mapper = mapper;
		}
		public async Task<ResponseMethod<ReportModel>> Handle(GetByReportIdQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ReportModel> result = new ResponseMethod<ReportModel>();
			var report = await _reportRepository.GetByIDAsync(request.Id);
			if (report == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.Data = _mapper.Map<ReportModel>(report);
			result.StatusCode = (int)HttpStatusCode.OK;
			return result;
		}
	}
}
