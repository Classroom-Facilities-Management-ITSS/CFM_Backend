﻿using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Classroom
{
	public class GetByClassroomNumberQuery : IRequest<ResponseMethod<ClassroomModel>>
	{
		public string ClassNumber { get; set; }
	}

	public class GetByClassroomNumberQueryHandler : IRequestHandler<GetByClassroomNumberQuery, ResponseMethod<ClassroomModel>>
	{
		private readonly IMapper _mapper;
		private readonly IClassroomRepository _classroomRepository;

		public GetByClassroomNumberQueryHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			_mapper = mapper;
			_classroomRepository = classroomRepository;
		}
        public async Task<ResponseMethod<ClassroomModel>> Handle(GetByClassroomNumberQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ClassroomModel> result = new ResponseMethod<ClassroomModel>();
			var classroom = await _classroomRepository.GetByAddressAsync(request.ClassNumber);
			if (classroom == null)
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
