﻿using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Classroom
{
    public class GetClassroomByIdQuery : IRequest<ResponseMethod<ClassroomModel>>
	{
		public Guid Id { get; set; }
	}

	public class GetByIdQueryHandler : IRequestHandler<GetClassroomByIdQuery, ResponseMethod<ClassroomModel>>
	{
		private readonly IMapper _mapper;
		private readonly IClassroomRepository _classroomRepository;

		public GetByIdQueryHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			_mapper = mapper;
			_classroomRepository = classroomRepository;
		}
        public async Task<ResponseMethod<ClassroomModel>> Handle(GetClassroomByIdQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ClassroomModel> result = new ResponseMethod<ClassroomModel>();
			//TODO: query including Facilities ID and Reports ID
			var classroom = await _classroomRepository.Queryable
				.Include(c => c.Facilities)
				.Include(c => c.Reports)
				.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
				.ConfigureAwait(false);
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
