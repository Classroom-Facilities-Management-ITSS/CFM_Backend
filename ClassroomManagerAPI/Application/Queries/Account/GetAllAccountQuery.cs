using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Account
{
    public class GetAllAccountQuery : FilterModel, IRequest<ResponseMethod<IEnumerable<AccountModel>>>
	{
	}
	public class GetAllQueryHandler : IRequestHandler<GetAllAccountQuery, ResponseMethod<IEnumerable<AccountModel>>>
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IMapper _mapper;
		public GetAllQueryHandler(IMapper mapper, IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
			_mapper = mapper;
		}

		public async Task<ResponseMethod<IEnumerable<AccountModel>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<AccountModel>> result = new ResponseMethod<IEnumerable<AccountModel>>();
			var account = await _accountRepository.Queryable.Include(x => x.UserInfo).Where(x => !x.IsDeleted).ToListAsync().ConfigureAwait(false);
			var accountResult = _accountRepository.GetPaginationEntity(account, request.page, request.limit);
			result.Data = _mapper.Map<IEnumerable<AccountModel>>(accountResult);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(await _accountRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(request);
			return result;
		}
	}
}
