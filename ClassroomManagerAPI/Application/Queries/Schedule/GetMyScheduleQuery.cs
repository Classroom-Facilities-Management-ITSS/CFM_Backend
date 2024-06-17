using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Schedule
{
    public class GetMyScheduleQuery : FilterModel , IRequest<ResponseMethod<IEnumerable<ScheduleModel>>>
    {
    }

    public class GetMyScheduleQueryHandler : IRequestHandler<GetMyScheduleQuery, ResponseMethod<IEnumerable<ScheduleModel>>>
    {
        private readonly AuthContext _authContex;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IMapper _mapper;

        public GetMyScheduleQueryHandler(AuthContext authContext, IScheduleRepository scheduleRepository, IMapper mapper)
        {
            _authContex = authContext;
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
        }
        public async Task<ResponseMethod<IEnumerable<ScheduleModel>>> Handle(GetMyScheduleQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<IEnumerable<ScheduleModel>>();
            var account_id = _authContex.GetCurrentId();
            if (account_id == Guid.Empty)
            {
                result.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
                result.StatusCode = StatusCodes.Status403Forbidden;
                return result;
            }

            var schedules = _scheduleRepository.Queryable.Where(x => x.AccountId == account_id);
            var scheduleResult = _scheduleRepository.GetPaginationEntity(schedules, request.page, request.limit);
            result.Data = _mapper.Map<IEnumerable<ScheduleModel>>(scheduleResult);
            result.StatusCode = (int)HttpStatusCode.OK;
            result.AddPagination(_scheduleRepository.PaginationEntity(schedules, request.page, request.limit));
            result.AddFilter(request);
            return result;
        }
    }
}
