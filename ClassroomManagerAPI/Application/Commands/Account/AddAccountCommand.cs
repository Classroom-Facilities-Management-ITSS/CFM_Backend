﻿using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Account
{
	public class AddAccountCommand : AddAccountModel, IRequest<Response<AccountModel>>
	{
	}

	public class AddAccountCommandHandler : IRequestHandler<AddAccountCommand, Response<AccountModel>>
	{
		private readonly IMapper _mapper;
		private readonly IAccountRepository _accountRepository;

		public AddAccountCommandHandler(IMapper mapper, IAccountRepository accountRepository)
        {
			_mapper = mapper;
			_accountRepository = accountRepository;
		}
        public async Task<Response<AccountModel>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<AccountModel> result = new Response<AccountModel>();
			var newAccount = _mapper.Map<Entities.Account>(request);
			var createdAccount = await _accountRepository.AddAsync(newAccount).ConfigureAwait(false);
			result.StatusCode = (int)HttpStatusCode.Created;
			result.Data = _mapper.Map<AccountModel>(createdAccount);
			return result;

		}
	}
}
