using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Account
{
    public class GetAccountByIdQuery : IRequest<ResponseMethod<AccountModel>>
	{
		public Guid Id { get; set; }
	}

	public class GetByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, ResponseMethod<AccountModel>>
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IMapper _mapper;
		public GetByIdQueryHandler(IMapper mapper, IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
			_mapper = mapper;
		}
		public async Task<ResponseMethod<AccountModel>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<AccountModel> result = new ResponseMethod<AccountModel>();
			var account = await _accountRepository.Queryable.Include(x =>x.UserInfo).FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
			if (account == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.Data = _mapper.Map<AccountModel>(account);
            result.StatusCode = (int)HttpStatusCode.OK;
			return result;
		}
	}
}
