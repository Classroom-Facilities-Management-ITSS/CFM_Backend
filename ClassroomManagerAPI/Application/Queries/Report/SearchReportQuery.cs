using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Report
{
    public class SearchReportQuery : SearchReportModel, IRequest<ResponseMethod<IEnumerable<ReportModel>>>
	{

    }

	public class SearchReportQueryHandler : IRequestHandler<SearchReportQuery, ResponseMethod<IEnumerable<ReportModel>>>
	{
		private readonly IMapper _mapper;
		private readonly IReportRepository _reportRepository;

		public SearchReportQueryHandler(IMapper mapper, IReportRepository reportRepository)
        {
			_mapper = mapper;
			_reportRepository = reportRepository;
		}
        public async Task<ResponseMethod<IEnumerable<ReportModel>>> Handle(SearchReportQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<ReportModel>> result = new ResponseMethod<IEnumerable<ReportModel>>();
			var reports = _reportRepository.Queryable().Include(x => x.Classroom).Include(x => x.Account).AsQueryable();
			if (request.ClassroomAddress != null)
			{
				reports = reports.Where(x => !x.IsDeleted && x.Classroom.Address.ToLower().Trim().Contains(request.ClassroomAddress.ToLower().Trim()));
			}
			if (request.Email != null)
			{
				reports = reports.Where(x => !x.IsDeleted && x.Account.Email.ToLower().Trim().Contains(request.Email.ToLower().Trim()));
			}
			if (request.FullName != null)
			{
				reports = reports.Where(x => !x.IsDeleted && x.Account.UserInfo.FullName.ToLower().Trim().Contains(request.FullName.ToLower().Trim()));	
			}
			if (request.Note != null)
			{
				reports = reports.Where(x => !x.IsDeleted && x.Note.ToLower().Trim().Contains(request.Note.ToLower().Trim()));
			}
			var reportsResult = _reportRepository.GetPaginationEntity(reports, request.page, request.limit);

			result.Data = _mapper.Map<IEnumerable<ReportModel>>(reportsResult);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(_reportRepository.PaginationEntity(reports, request.page, request.limit));
			result.AddFilter(request);
			return result;
		}
	}
}
