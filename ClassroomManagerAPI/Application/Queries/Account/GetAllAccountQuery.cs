using AutoMapper;
using ClassroomManagerAPI.Application.Queries.Facility;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Account
{
	public class GetAllAccountQuery : FilterModel, IRequest<Response<IEnumerable<AccountModel>>>
	{
	}
	public class GetAllQueryHandler : IRequestHandler<GetAllAccountQuery, Response<IEnumerable<AccountModel>>>
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IMapper _mapper;
		public GetAllQueryHandler(IMapper mapper, IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
			_mapper = mapper;
		}

		public async Task<Response<IEnumerable<AccountModel>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<IEnumerable<AccountModel>> result = new Response<IEnumerable<AccountModel>>();
			var accountResult = await _accountRepository.GetAllAsync(request.page, request.limit).ConfigureAwait(false);
			result.Data = _mapper.Map<IEnumerable<AccountModel>>(accountResult);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.AddPagination(await _accountRepository.Pagination(request.page, request.limit).ConfigureAwait(false));
			result.AddFilter(request);
			return result;
		}
	}
}
