﻿using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Account
{
	public class DeleteAccountCommand : IRequest<Response<string>>
	{
		public Guid Id { get; set; }
	}
	public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Response<string>>
	{
		private readonly IMapper mapper;
		private readonly IAccountRepository accountRepository;

		public DeleteAccountCommandHandler(IMapper mapper, IAccountRepository accountRepository)
		{
			this.mapper = mapper;
			this.accountRepository = accountRepository;
		}

		public async Task<Response<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<string> result = new Response<string>();
			var deletedFacility = await accountRepository.DeleteAsync(request.Id).ConfigureAwait(false);
			if (!deletedFacility)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"Delete account with id {request.Id} sucessfully";
			return result;
		}
	}
}
