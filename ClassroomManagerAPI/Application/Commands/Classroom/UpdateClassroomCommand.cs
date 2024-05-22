using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
	public class UpdateClassroomCommand : UpdateClassroomModel, IRequest<ResponseMethod<string>>
	{
	}

	public class UpdateClassroomCommandHandler : IRequestHandler<UpdateClassroomCommand, ResponseMethod<string>>
	{
		private readonly IMapper mapper;
		private readonly IClassroomRepository classroomRepository;

		public UpdateClassroomCommandHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			this.mapper = mapper;
			this.classroomRepository = classroomRepository;
		}

		public async Task<ResponseMethod<string>> Handle(UpdateClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var classroom = mapper.Map<Entities.Classroom>(request);
			var updatedClassroom = await classroomRepository.UpdateAsync(classroom).ConfigureAwait(false);
			if (updatedClassroom == null)
			{
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"update classroom with id {request.Id} sucessfully";
			return result;
		}
	}
}
