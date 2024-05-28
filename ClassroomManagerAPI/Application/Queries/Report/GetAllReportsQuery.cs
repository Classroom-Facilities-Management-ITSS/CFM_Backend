using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Report
{
    public class GetAllReportsQuery : FilterModel, IRequest<ResponseMethod<IEnumerable<ReportModel>>>
	{
	}

	public class GetAllQueryHandler : IRequestHandler<GetAllReportsQuery, ResponseMethod<IEnumerable<ReportModel>>>
	{
		private readonly IReportRepository _reportRepository;
		private readonly IMapper _mapper;
		public GetAllQueryHandler(IMapper mapper, IReportRepository reportRepository)
		{
			_reportRepository = reportRepository;
			_mapper = mapper;
		}
		public async Task<ResponseMethod<IEnumerable<ReportModel>>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<ReportModel>> result = new ResponseMethod<IEnumerable<ReportModel>>();
			var report = await _reportRepository.GetAllAsync(request.page, request.limit).ConfigureAwait(false);
			result.Data = _mapper.Map<IEnumerable<ReportModel>>(report);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(await _reportRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(request);
			return result;
		}
	}
}
