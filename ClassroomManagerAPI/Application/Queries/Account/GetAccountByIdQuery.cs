using AutoMapper;
using ClassroomManagerAPI.Application.Queries.Facility;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Queries.Account
{
	public class GetAccountByIdQuery : IRequest<Response<AccountModel>>
	{
		public Guid Id { get; set; }
	}

	public class GetByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Response<AccountModel>>
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IMapper _mapper;
		public GetByIdQueryHandler(IMapper mapper, IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
			_mapper = mapper;
		}
		public async Task<Response<AccountModel>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<AccountModel> result = new Response<AccountModel>();
			var facility = await _accountRepository.GetByIDAsync(request.Id);
			if (facility == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.Data = _mapper.Map<AccountModel>(facility);
			result.StatusCode = (int)HttpStatusCode.OK;
			return result;
		}
	}
}
