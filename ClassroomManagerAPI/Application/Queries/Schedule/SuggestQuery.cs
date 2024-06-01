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
	public class SuggestQuery : SuggestModel,  IRequest<ResponseMethod<IEnumerable<ClassroomModel>>>
	{

    }

	public class SuggestQueryHandler : IRequestHandler<SuggestQuery, ResponseMethod<IEnumerable<ClassroomModel>>>
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
        public async Task<ResponseMethod<IEnumerable<ClassroomModel>>> Handle(SuggestQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<ClassroomModel>> result = new ResponseMethod<IEnumerable<ClassroomModel>>();
			var scheduleByID = await _scheduleRepository.GetByIDAsync(request.ScheduleId);
			if (scheduleByID == null)
			{
				result.StatusCode = (int)HttpStatusCode.NotFound;
				result.Message = "Schedule not found";
				return result;
			}
			var targetClassrooms = await _classroomRepository.Queryable().Include(c => c.Facilities).FirstOrDefaultAsync(c => c.Id == scheduleByID.ClassroomId);
			var targetFacilities = targetClassrooms.Facilities;
			var classrooms = _classroomRepository.Queryable().Include(c => c.Facilities).Where(x => !x.IsDeleted && 
																								x.MaxSize * 1.2 >= targetClassrooms.MaxSize && 
																								x.FacilityAmount <= targetClassrooms.FacilityAmount && 
																								x.Facilities.Any(f => targetFacilities.
																									Any(tf => tf.Name == f.Name && tf.Count >= f.Count))).AsQueryable();

			// if the schedule.studentcount of any schedule is >= the schedule.studentcount of the schedule with the input id, return it
			// if the schedule.facilityamount of any schedule is >= the schedule.facilityamount of the schedule with the input id, return it
			var classroomResult = _classroomRepository.GetPaginationEntity(classrooms, request.page, request.limit);

			result.Data = _mapper.Map<IEnumerable<ClassroomModel>>(classroomResult);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(_classroomRepository.PaginationEntity(classrooms, request.page, request.limit));
			result.AddFilter(request);
			return result;
		}
	}
}
