using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories;
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
			var reports = _reportRepository.Queryable().Include(x => x.Classroom).Include(x => x.Account).AsQueryable();
			if (request.ClassroomAddress != null)
			{
				reports = reports.Where(x => !x.IsDeleted && x.Classroom.Address.ToLower().Trim().Contains(request.ClassroomAddress.ToLower().Trim()));
			}
			if (request.AccountEmail != null)
			{
				reports = reports.Where(x => !x.IsDeleted && x.Account.Email.ToLower().Trim().Contains(request.AccountEmail.ToLower().Trim()));
			}
			if (request.UserName != null)
			{
				reports = reports.Where(x => !x.IsDeleted && x.Account.UserInfo.FullName.ToLower().Trim().Contains(request.UserName.ToLower().Trim()));	
			}
			if (request.Note != null)
			{
				reports = reports.Where(x => !x.IsDeleted && x.Note.ToLower().Trim().Contains(request.Note.ToLower().Trim()));
			}
			var reportsResult = _reportRepository.GetPaginationEntity(reports, request.page, request.limit);

			result.Data = _mapper.Map<IEnumerable<ReportModel>>(reports);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(await _reportRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(new FilterModel { page = request.page, limit = request.limit });
			return result;
		}
	}
}
