﻿using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Classroom
{
    public class SearchClassroomQuery : SearchClassroomModel, IRequest<ResponseMethod<ClassroomModel>>
	{

	}

	public class GetByClassroomNumberQueryHandler : IRequestHandler<SearchClassroomQuery, ResponseMethod<ClassroomModel>>
	{
		private readonly IMapper _mapper;
		private readonly IClassroomRepository _classroomRepository;

		public GetByClassroomNumberQueryHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			_mapper = mapper;
			_classroomRepository = classroomRepository;
		}
        public async Task<ResponseMethod<ClassroomModel>> Handle(SearchClassroomQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ClassroomModel> result = new ResponseMethod<ClassroomModel>();
			var classroom = _classroomRepository.Queryable().AsQueryable();
			if (request.ClassroomAddress != null)
			{
				classroom = classroom.Where(x => !x.IsDeleted && x.Address.ToLower().Trim().Contains(request.ClassroomAddress.ToLower().Trim()));
			}
			if (request.LastUsed != null)
			{
				classroom = classroom.Where(x => !x.IsDeleted && x.LastUsed > request.LastUsed);
			}
			if (request.FacilityAmount != null)
			{
				classroom = classroom.Where(x => !x.IsDeleted && x.FacilityAmount > request.FacilityAmount);
			}
			if (request.Note != null)
			{
				classroom = classroom.Where(x => !x.IsDeleted && x.Note.ToLower().Trim().Contains(request.Note.ToLower().Trim()));
			}
			if (request.Status != null)
			{
				classroom = classroom.Where(x => !x.IsDeleted && x.Status == request.Status);
			}

			var classroomResult = _classroomRepository.GetPaginationEntity(classroom, request.page, request.limit);

			result.Data = _mapper.Map<ClassroomModel>(classroom);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(_classroomRepository.PaginationEntity(classroom, request.page, request.limit));
			result.AddFilter(request);
			return result;
		}
	}
}
