using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
    public class UpdateClassroomCommand : UpdateClassroomModel, IRequest<ResponseMethod<ClassroomModel>>
	{
	}

	public class UpdateClassroomCommandHandler : IRequestHandler<UpdateClassroomCommand, ResponseMethod<ClassroomModel>>
	{
		private readonly IMapper _mapper;
		private readonly IClassroomRepository _classroomRepository;

		public UpdateClassroomCommandHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			_mapper = mapper;
			_classroomRepository = classroomRepository;
		}

		public async Task<ResponseMethod<ClassroomModel>> Handle(UpdateClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ClassroomModel> result = new ResponseMethod<ClassroomModel>();
			var classroom = _mapper.Map<Entities.Classroom>(request);
			var updatedClassroom = await _classroomRepository.UpdateAsync(classroom).ConfigureAwait(false);
			if (updatedClassroom == null)
			{
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = _mapper.Map<ClassroomModel>(updatedClassroom);
			return result;
		}
	}
}
