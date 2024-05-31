using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Schedule
{
	public class SuggestQuery : SuggestModel,  IRequest<ResponseMethod<IEnumerable<ScheduleModel>>>
	{

    }

	public class SuggestQueryHandler : IRequestHandler<SuggestQuery, ResponseMethod<IEnumerable<ScheduleModel>>>
	{
		private readonly IMapper _mapper;
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IClassroomRepository _classroomRepository;
		private readonly IFacilityRepository _facilityRepository;

		public SuggestQueryHandler(IMapper mapper, IScheduleRepository scheduleRepository, IClassroomRepository classroomRepository, IFacilityRepository facilityRepository)
        {
			_mapper = mapper;
			_scheduleRepository = scheduleRepository;
			_classroomRepository = classroomRepository;
			_facilityRepository = facilityRepository;
		}
        public async Task<ResponseMethod<IEnumerable<ScheduleModel>>> Handle(SuggestQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<ScheduleModel>> result = new ResponseMethod<IEnumerable<ScheduleModel>>();
			var scheduleByID = await _scheduleRepository.GetByIDAsync(request.ScheduleId); 
			var schedules = _scheduleRepository.Queryable().AsQueryable();
			var classrooms = await _classroomRepository.Queryable().Include(c => c.Facilities).AsQueryable().FirstOrDefaultAsync(c => c.Id == scheduleByID.ClassroomId);
			var facilities = classrooms.Facilities;
			// if the schedule.studentcount of any schedule is >= the schedule.studentcount of the schedule with the input id, return it
			// if the schedule.facilityamount of any schedule is >= the schedule.facilityamount of the schedule with the input id, return it
			schedules = schedules.Where(x => !x.IsDeleted && x.CountStudent <= scheduleByID.CountStudent && x.Classroom.MaxSize * 1.2 >= scheduleByID.Classroom.MaxSize && x.Classroom.FacilityAmount <= scheduleByID.Classroom.FacilityAmount && x.Classroom.Facilities.Any(f => facilities.Any(tf => tf.Name == f.Name && tf.Count >= f.Count))).AsQueryable();
			var schedulesList = await schedules.ToListAsync();
			var scheduleModels = _mapper.Map<IEnumerable<ScheduleModel>>(schedulesList);

			result.Data = scheduleModels;
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(_scheduleRepository.PaginationEntity(schedulesList, request.page, request.limit));
			result.AddFilter(request);
			return result;
		}
	}
}
