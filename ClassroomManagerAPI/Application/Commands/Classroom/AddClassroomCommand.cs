using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
	public class AddClassroomCommand : AddClassroomModel, IRequest<Response<ClassroomModel>>
	{
	}

	public class AddClassroomCommandHandler : IRequestHandler<AddClassroomCommand, Response<ClassroomModel>>
	{
		private readonly IMapper mapper;
		private readonly IClassroomRepository classroomRepository;

		public AddClassroomCommandHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			this.mapper = mapper;
			this.classroomRepository = classroomRepository;
		}
        public async Task<Response<ClassroomModel>> Handle(AddClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<ClassroomModel> result = new Response<ClassroomModel>();
			var newClassroom = mapper.Map<Entities.Classroom>(request);
			var createdClassroom = await classroomRepository.AddAsync(newClassroom).ConfigureAwait(false);
			result.StatusCode = (int)HttpStatusCode.Created;
			result.Data = mapper.Map<ClassroomModel>(createdClassroom);
			return result;
		}
	}
}
