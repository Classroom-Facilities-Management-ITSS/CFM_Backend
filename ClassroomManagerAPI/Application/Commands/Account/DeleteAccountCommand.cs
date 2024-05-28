using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Account
{
    public class DeleteAccountCommand : IRequest<ResponseMethod<string>>
	{
		public Guid Id { get; set; }
	}
	public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, ResponseMethod<string>>
	{
		private readonly IAccountRepository _accountRepository;

		public DeleteAccountCommandHandler(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		public async Task<ResponseMethod<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var deletedAccount = await _accountRepository.DeleteAsync(request.Id).ConfigureAwait(false);
			if (!deletedAccount)
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
