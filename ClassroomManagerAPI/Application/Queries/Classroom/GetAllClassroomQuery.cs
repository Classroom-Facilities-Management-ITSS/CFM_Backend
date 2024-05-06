using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Classroom
{
	public class GetAllClassroomQuery : FilterModel, IRequest<Response<IEnumerable<ClassroomModel>>>
	{
	}

	public class GetAllQueryHandler : IRequestHandler<GetAllClassroomQuery, Response<IEnumerable<ClassroomModel>>>
	{
		private readonly IClassroomRepository _classroomRepository;
		private readonly IMapper _mapper;
        public GetAllQueryHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
			_mapper = mapper;
        }
        public async Task<Response<IEnumerable<ClassroomModel>>> Handle(GetAllClassroomQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<IEnumerable<ClassroomModel>> result = new Response<IEnumerable<ClassroomModel>>();
			var classroomResult = await _classroomRepository.GetAllAsync(request.page, request.limit).ConfigureAwait(false);
			result.Data = _mapper.Map<IEnumerable<ClassroomModel>>(classroomResult);
			result.StatusCode = (int) HttpStatusCode.OK;
			result.AddPagination(await _classroomRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(request);
			return result;
		}
	}
}
