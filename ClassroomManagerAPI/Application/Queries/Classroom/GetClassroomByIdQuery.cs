using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Classroom
{
	public class GetClassroomByIdQuery : IRequest<Response<ClassroomModel>>
	{
		public Guid Id { get; set; }
	}

	public class GetByIdQueryHandler : IRequestHandler<GetClassroomByIdQuery, Response<ClassroomModel>>
	{
		private readonly IMapper _mapper;
		private readonly IClassroomRepository _classroomRepository;

		public GetByIdQueryHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			_mapper = mapper;
			_classroomRepository = classroomRepository;
		}
        public async Task<Response<ClassroomModel>> Handle(GetClassroomByIdQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<ClassroomModel> result = new Response<ClassroomModel>();
			var classroom = await _classroomRepository.GetByIDAsync(request.Id);
			if(classroom == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.Data = _mapper.Map<ClassroomModel>(classroom);
			result.StatusCode = (int)HttpStatusCode.OK;
			return result;
		}
	}
}
