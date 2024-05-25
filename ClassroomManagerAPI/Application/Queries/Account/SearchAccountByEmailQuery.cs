using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Repositories;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Account
{
	public class SearchAccountByEmailQuery : FilterModel, IRequest<Response<AccountModel>>
	{
        public string email { get; set; }
    }

	public class SearchAccountQueryHandler : IRequestHandler<SearchAccountByEmailQuery, Response<AccountModel>>
	{
		private readonly IMapper _mapper;
		private readonly IAccountRepository _accountRepository;

		public SearchAccountQueryHandler(IMapper mapper, IAccountRepository accountRepository)
        {
			_mapper = mapper;
			_accountRepository = accountRepository;
		}
        public async Task<Response<AccountModel>> Handle(SearchAccountByEmailQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<AccountModel> result = new Response<AccountModel>();
			var account = await _accountRepository.GetByEmailAsync(request.email).ConfigureAwait(false);
			if (account == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = StatusCodes.Status404NotFound;
				return result;
			}
			result.Data = _mapper.Map<AccountModel>(account);
			result.StatusCode = (int)HttpStatusCode.OK;
			return result;
		}
	}
}
