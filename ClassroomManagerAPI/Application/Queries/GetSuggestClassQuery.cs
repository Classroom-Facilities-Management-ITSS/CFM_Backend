using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;

namespace ClassroomManagerAPI.Application.Queries
{
    public class GetSuggestClassQuery : IRequest<ResponseMethod<IEnumerable<ClassroomModel>>>
    {
        public Guid? Id { get; set; }
    }

    public class GetSuggestClassQueryHandle : IRequestHandler<GetSuggestClassQuery, ResponseMethod<IEnumerable<ClassroomModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IClassroomRepository _classroomRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IFacilityRepository _facilityRepository;

        public GetSuggestClassQueryHandle(IMapper mapper, IClassroomRepository classroomRepository, IScheduleRepository scheduleRepository, IFacilityRepository facilityRepository)
        {
            _mapper = mapper;
            _classroomRepository = classroomRepository;
            _scheduleRepository = scheduleRepository;
            _facilityRepository = facilityRepository;
        }
        public Task<ResponseMethod<IEnumerable<ClassroomModel>>> Handle(GetSuggestClassQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
