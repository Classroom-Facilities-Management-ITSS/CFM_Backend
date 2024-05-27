using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Report
{
	public class SearchReportQuery : FilterModel, IRequest<ResponseMethod<IEnumerable<ReportModel>>>
	{
        public string ClassNumber { get; set; }
    }

	public class SearchReportQueryHandler : IRequestHandler<SearchReportQuery, ResponseMethod<IEnumerable<ReportModel>>>
	{
		private readonly IMapper _mapper;
		private readonly IReportRepository _reportRepository;
		private readonly IClassroomRepository _classroomRepository;

		public SearchReportQueryHandler(IMapper mapper, IReportRepository reportRepository, IClassroomRepository classroomRepository)
        {
			_mapper = mapper;
			_reportRepository = reportRepository;
			_classroomRepository = classroomRepository;
		}
        public async Task<ResponseMethod<IEnumerable<ReportModel>>> Handle(SearchReportQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<ReportModel>> result = new ResponseMethod<IEnumerable<ReportModel>>();
			var reports = await _reportRepository.GetByClassroomAddressAsync(request.ClassNumber, request.page, request.limit);
			
			result.Data = _mapper.Map<IEnumerable<ReportModel>>(reports);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(await _reportRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(new FilterModel { page = request.page, limit = request.limit });
			return result;
		}
	}
}
