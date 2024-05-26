﻿using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Classroom
{
	public class DeleteClassroomCommand : IRequest<ResponseMethod<string>>
	{
		public Guid Id { get; set; }
	}
	public class DeleteClassroomCommandHandler : IRequestHandler<DeleteClassroomCommand, ResponseMethod<string>>
	{
		private readonly IMapper mapper;
		private readonly IClassroomRepository classroomRepository;

		public DeleteClassroomCommandHandler(IMapper mapper, IClassroomRepository classroomRepository)
        {
			this.mapper = mapper;
			this.classroomRepository = classroomRepository;
		}

		public async Task<ResponseMethod<string>> Handle(DeleteClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var deletedClassrooom = await classroomRepository.DeleteAsync(request.Id).ConfigureAwait(false);
			if (!deletedClassrooom)
			{
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"Delete classroom with id {request.Id} sucessfully";
			return result;
		}
	}
}