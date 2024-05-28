using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.User;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Account
{
    public class SearchAccountQuery : SearchAccountModel, IRequest<ResponseMethod<IEnumerable<AccountModel>>>
	{
    }

	public class SearchAccountQueryHandler : IRequestHandler<SearchAccountQuery, ResponseMethod<IEnumerable<AccountModel>>>
	{
		private readonly IMapper _mapper;
		private readonly IAccountRepository _accountRepository;

		public SearchAccountQueryHandler(IMapper mapper, IAccountRepository accountRepository)
        {
			_mapper = mapper;
			_accountRepository = accountRepository;
		}
        public async Task<ResponseMethod<IEnumerable<AccountModel>>> Handle(SearchAccountQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<IEnumerable<AccountModel>> result = new ResponseMethod<IEnumerable<AccountModel>>();
			var account =  _accountRepository.Queryable().Include(x => x.UserInfo).AsQueryable();
			if(request.FullName != null) {
				account = account.Where(x => !x.IsDeleted && x.UserInfo.FullName.ToLower().Trim().Contains(request.FullName.ToLower().Trim()));
			}

			if(request.Email != null)
			{
				account = account.Where(x => !x.IsDeleted && x.Email.ToLower().Trim().Contains(request.Email.ToLower().Trim()));
            }

			var accountResult = _accountRepository.GetPaginationEntity(account, request.page, request.limit);

			result.Data = _mapper.Map<IEnumerable<AccountModel>>(accountResult);
			result.StatusCode = (int)HttpStatusCode.OK;
            result.AddPagination(_accountRepository.PaginationEntity(account, request.page, request.limit));
            result.AddFilter(request);
            return result;
		}
	}
}
