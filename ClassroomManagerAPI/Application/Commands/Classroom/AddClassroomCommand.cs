using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
    public class AddClassroomCommand : AddClassroomModel, IRequest<ResponseMethod<ClassroomModel>>
	{
	}

	public class AddClassroomCommandHandler : IRequestHandler<AddClassroomCommand, ResponseMethod<ClassroomModel>>
	{
		private readonly IMapper _mapper;
		private readonly IClassroomRepository _classroomRepository;

		public AddClassroomCommandHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			_mapper = mapper;
			_classroomRepository = classroomRepository;
		}
        public async Task<ResponseMethod<ClassroomModel>> Handle(AddClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ClassroomModel> result = new ResponseMethod<ClassroomModel>();
			var newClassroom = _mapper.Map<Entities.Classroom>(request);
			var existingClassroom = await _classroomRepository.Queryable()
				.Where(a => a.Address == request.Address && !a.IsDeleted)
				.FirstOrDefaultAsync();
			if (existingClassroom != null)
			{
				result.AddBadRequest(nameof(ErrorClassEnum.ClassroomAlreadyExist));
				result.StatusCode = (int)HttpStatusCode.Conflict;
				return result;
			}
			var createdClassroom = await _classroomRepository.AddAsync(newClassroom).ConfigureAwait(false);
			result.StatusCode = (int)HttpStatusCode.Created;
			result.Data = _mapper.Map<ClassroomModel>(createdClassroom);
			return result;
		}
	}
}
