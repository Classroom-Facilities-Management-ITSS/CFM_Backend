using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Classroom
{
	public class GetByClassroomNumberQuery : IRequest<Response<ClassroomModel>>
	{
		public string ClassNumber { get; set; }
	}

	public class GetByClassroomNumberQueryHandler : IRequestHandler<GetByClassroomNumberQuery, Response<ClassroomModel>>
	{
		private readonly IMapper _mapper;
		private readonly IClassroomRepository _classroomRepository;

		public GetByClassroomNumberQueryHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			_mapper = mapper;
			_classroomRepository = classroomRepository;
		}
        public async Task<Response<ClassroomModel>> Handle(GetByClassroomNumberQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<ClassroomModel> result = new Response<ClassroomModel>();
			var classroom = await _classroomRepository.GetByAddressAsync(request.ClassNumber);
			if (classroom == null)
			{
				result.AddBadRequest($"Classroomo with number {request.ClassNumber} not existing");
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.Data = _mapper.Map<ClassroomModel>(classroom);
			result.StatusCode = (int)HttpStatusCode.OK;
			return result;
		}
	}
}
