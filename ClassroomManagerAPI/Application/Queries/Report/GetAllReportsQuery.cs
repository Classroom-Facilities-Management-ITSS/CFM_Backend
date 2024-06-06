using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
			var result = new ResponseMethod<IEnumerable<ReportModel>>();
			var reports = await _reportRepository.Queryable
				.Include(x => x.Account)
				.ThenInclude(x => x.UserInfo)
				.Include(x => x.Classroom)
                .Where(x => !x.IsDeleted)
                .ToListAsync().ConfigureAwait(false);
			var report = _reportRepository.GetPaginationEntity(reports, request.page, request.limit);
			result.Data = _mapper.Map<IEnumerable<ReportModel>>(report);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(await _reportRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(request);
			return result;
		}
	}
}
