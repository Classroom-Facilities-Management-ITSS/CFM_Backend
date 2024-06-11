using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Report
{
    public class GetMyReportQuery : FilterModel, IRequest<ResponseMethod<IEnumerable<ReportModel>>>
    {
    }

    public class GetMyReportQueryHandler : IRequestHandler<GetMyReportQuery, ResponseMethod<IEnumerable<ReportModel>>>
    {
        private readonly AuthContext _authContex;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GetMyReportQueryHandler(AuthContext authContext, IReportRepository reportRepository, IMapper mapper)
        {
            _authContex = authContext;
            _reportRepository = reportRepository;
            _mapper = mapper;
        }
        public async Task<ResponseMethod<IEnumerable<ReportModel>>> Handle(GetMyReportQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<IEnumerable<ReportModel>>();
            var account_id = _authContex.GetCurrentId();
            if(account_id == Guid.Empty)
            {
                result.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
                result.StatusCode = StatusCodes.Status403Forbidden;
                return result;
            }

            var reports = _reportRepository.Queryable.Where(x => x.AccountId == account_id);
            var reportsResult = _reportRepository.GetPaginationEntity(reports, request.page, request.limit);
            result.Data = _mapper.Map<IEnumerable<ReportModel>>(reportsResult);
            result.StatusCode = (int)HttpStatusCode.OK;
            result.AddPagination(_reportRepository.PaginationEntity(reports, request.page, request.limit));
            result.AddFilter(request);
            return result;
        }
    }
}
