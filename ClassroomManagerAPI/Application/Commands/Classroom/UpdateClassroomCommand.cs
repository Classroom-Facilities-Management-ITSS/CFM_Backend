using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
	public class UpdateClassroomCommand : UpdateClassroomModel, IRequest<Response<string>>
	{
	}

	public class UpdateClassroomCommandHandler : IRequestHandler<UpdateClassroomCommand, Response<string>>
	{
		private readonly IMapper mapper;
		private readonly IClassroomRepository classroomRepository;

		public UpdateClassroomCommandHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			this.mapper = mapper;
			this.classroomRepository = classroomRepository;
		}

		public async Task<Response<string>> Handle(UpdateClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<string> result = new Response<string>();
			var classroom = mapper.Map<Entities.Classroom>(request);
			var updatedClassroom = await classroomRepository.UpdateAsync(classroom).ConfigureAwait(false);
			if (updatedClassroom == null)
			{
				result.AddBadRequest($"Classroom with id {request.Id} not existing");
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"update classroom with id {request.Id} sucessfully";
			return result;
		}
	}
}
