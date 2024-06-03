using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Application.Queries
{
    public class GetSuggestClassQuery : IRequest<ResponseMethod<IEnumerable<ClassroomModel>>>
    {
        public Guid Id { get; set; }
    }

    public class GetSuggestClassQueryHandle : IRequestHandler<GetSuggestClassQuery, ResponseMethod<IEnumerable<ClassroomModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IClassroomRepository _classroomRepository;
        private readonly IFacilityRepository _facilityRepository;

        public GetSuggestClassQueryHandle(IMapper mapper, IClassroomRepository classroomRepository, IFacilityRepository facilityRepository)
        {
            _mapper = mapper;
            _classroomRepository = classroomRepository;
            _facilityRepository = facilityRepository;
        }
        public async Task<ResponseMethod<IEnumerable<ClassroomModel>>> Handle(GetSuggestClassQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<IEnumerable<ClassroomModel>>();
            var classroom = await _classroomRepository.GetByIDAsync(request.Id).ConfigureAwait(false);
            if(classroom == null) {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = StatusCodes.Status404NotFound;
                return result;
            }

            var listSuggestion = _classroomRepository.Queryable().Include(x => x.Schedules).Include(x => x.Facilities).AsQueryable();
            listSuggestion = listSuggestion.Where(x => classroom.MaxSize >= x.MaxSize && x.MaxSize <= classroom.MaxSize * 1.2);
            listSuggestion = listSuggestion.Where(x => x.Schedules.Where(x => x.StartTime == DateTime.Now) == null);
            var facilities = _facilityRepository.Queryable().Where(x => x.ClassroomId == request.Id).AsQueryable();
            var listResult = new List<Entities.Classroom>();
            foreach (var item in listSuggestion)
            {
                bool found = true;
                foreach (var facility in facilities)
                {
                    if(!item.Facilities.Any(x => x.Name.ToLower().Trim().Equals(facility.Name.ToLower().Trim())))
                    {
                        found = false;
                        break;
                    }
                }
                if (found) listResult.Add(item);
            }

            result.Data = _mapper.Map<IEnumerable<ClassroomModel>>(listResult);
            result.StatusCode = StatusCodes.Status200OK;
            return result;
        }
    }
}
