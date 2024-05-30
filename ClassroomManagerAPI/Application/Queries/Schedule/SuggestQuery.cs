using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
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
			
			var classroomList = _classroomRepository.Queryable().AsQueryable();
			// if the number of facilities that are usable of any classroom is equal or higher than the number of facilities this current room has that are usable, return it
			// if the maxstudent any classroom has is equal or higher than the maxstudent of the current room, return it
			if (request.ClassroomId != null)
			{
				var usableFacilities = await _facilityRepository.CountFacilitiesByStatusAsync(request.ClassroomId, FacilityStatusEnum.NEW).ConfigureAwait(false);
				classroomList = classroomList.Where(x => !x.IsDeleted && x.Facilities.Where(f => f.Status == FacilityStatusEnum.NEW).Count() <= usableFacilities);
			}
			if (request.ClassroomAddress != null)
			{
				var usableFacilities = await _facilityRepository.CountFacilitiesByStatusAsync(request.ClassroomAddress, FacilityStatusEnum.NEW).ConfigureAwait(false);
				classroomList = classroomList.Where(x => !x.IsDeleted && x.Facilities.Where(f => f.Status == FacilityStatusEnum.NEW).Count() <= usableFacilities);
			}

			var classroomResult = _classroomRepository.GetPaginationEntity(classroomList, request.page, request.limit);

			result.Data = _mapper.Map<IEnumerable<ClassroomModel>>(classroomResult);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(_classroomRepository.PaginationEntity(classroomList, request.page, request.limit));
			result.AddFilter(request);
			return result;
		}
	}
}
