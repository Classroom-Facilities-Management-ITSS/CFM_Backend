using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Schedule
{
    public class SearchScheduleQuery : SearchScheduleModel, IRequest<ResponseMethod<IEnumerable<ScheduleModel>>>
	{
        
    }

	public class SearchScheduleQueryHandler : IRequestHandler<SearchScheduleQuery, ResponseMethod<IEnumerable<ScheduleModel>>>
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IMapper _mapper;
		public SearchScheduleQueryHandler(IMapper mapper, IScheduleRepository scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
			_mapper = mapper;
		}

		public async Task<ResponseMethod<IEnumerable<ScheduleModel>>> Handle(SearchScheduleQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<ScheduleModel>> result = new ResponseMethod<IEnumerable<ScheduleModel>>();
			var schedule = _scheduleRepository.Queryable.Include(x => x.Classroom).Include(x => x.Account).AsQueryable();
			if (request.ClassroomAddress != null)
			{
				schedule = schedule.Where(x => !x.IsDeleted && x.Classroom.Address.ToLower().Trim().Contains(request.ClassroomAddress.ToLower().Trim()));
			}

			if (request.Email != null)
			{
				schedule = schedule.Where(x => !x.IsDeleted && x.Account.Email.ToLower().Trim().Contains(request.Email.ToLower().Trim()));
			}
			if (request.FullName != null)
			{
				schedule = schedule.Where(x => !x.IsDeleted && x.Account.UserInfo.FullName.ToLower().Trim().Contains(request.FullName.ToLower().Trim()));
			}
			if(request.Date != null)
			{
				schedule = schedule.Where(x => !x.IsDeleted && DateOnly.FromDateTime(x.StartTime) == request.Date);
			}
			if (request.StartTime != null)
			{
				schedule = schedule.Where(x => !x.IsDeleted && TimeOnly.FromDateTime(x.StartTime) >= request.StartTime);
			}
			if (request.EndTime != null)
			{
				schedule = schedule.Where(x => !x.IsDeleted && request.EndTime <= TimeOnly.FromDateTime(x.EndTime));
			}
			if (request.CountStudent != null)
			{
				schedule = schedule.Where(x => !x.IsDeleted && x.CountStudent >= request.CountStudent);
			}
			if (request.Subject != null)
			{
				schedule = schedule.Where(x => !x.IsDeleted && x.Subject.ToLower().Trim().Contains(request.Subject.ToLower().Trim()));
			}

			var scheduleResult = _scheduleRepository.GetPaginationEntity(schedule, request.page, request.limit);

			result.Data = _mapper.Map<IEnumerable<ScheduleModel>>(scheduleResult);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(_scheduleRepository.PaginationEntity(schedule, request.page, request.limit));
			result.AddFilter(request);
			return result;
		}
	}
}
