using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
    public class AddClassroomCommand : AddClassroomModel, IRequest<ResponseMethod<ClassroomModel>>
	{
	}

	public class AddClassroomCommandHandler : IRequestHandler<AddClassroomCommand, ResponseMethod<ClassroomModel>>
	{
		private readonly IMapper mapper;
		private readonly IClassroomRepository classroomRepository;

		public AddClassroomCommandHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			this.mapper = mapper;
			this.classroomRepository = classroomRepository;
		}
        public async Task<ResponseMethod<ClassroomModel>> Handle(AddClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ClassroomModel> result = new ResponseMethod<ClassroomModel>();
			var newClassroom = mapper.Map<Entities.Classroom>(request);
			var createdClassroom = await classroomRepository.AddAsync(newClassroom).ConfigureAwait(false);
			result.StatusCode = (int)HttpStatusCode.Created;
			result.Data = mapper.Map<ClassroomModel>(createdClassroom);
			return result;
		}
	}
}
