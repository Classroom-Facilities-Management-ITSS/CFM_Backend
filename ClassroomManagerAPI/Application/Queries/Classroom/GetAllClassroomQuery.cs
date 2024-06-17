using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Classroom
{
	public class GetAllClassroomQuery : FilterModel, IRequest<ResponseMethod<IEnumerable<ClassroomModel>>>
	{
	}

	public class GetAllQueryHandler : IRequestHandler<GetAllClassroomQuery, ResponseMethod<IEnumerable<ClassroomModel>>>
	{
		private readonly IClassroomRepository _classroomRepository;
		private readonly IMapper _mapper;
        public GetAllQueryHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
			_mapper = mapper;
        }
        public async Task<ResponseMethod<IEnumerable<ClassroomModel>>> Handle(GetAllClassroomQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<ClassroomModel>> result = new ResponseMethod<IEnumerable<ClassroomModel>>();
			var classroomResult = await _classroomRepository.Queryable.Where(x => !x.IsDeleted && x.Id != Settings.StorageClassId).ToListAsync().ConfigureAwait(false);
			var classroom = _classroomRepository.GetPaginationEntity(classroomResult, request.page, request.limit);
			result.Data = _mapper.Map<IEnumerable<ClassroomModel>>(classroom);
			result.StatusCode = (int) HttpStatusCode.OK;
			result.AddPagination(_classroomRepository.PaginationEntity(classroom, request.page, request.limit));
			result.AddFilter(request);
			return result;
		}
	}
}
