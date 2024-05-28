using ClassroomManagerAPI.Application.Commands.Auth;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Account;
using MediatR;

namespace ClassroomManagerAPI.Application.Commands.Account
{
    public class AddAccountCommand : AddAccountModel, IRequest<ResponseMethod<AccountModel>>
	{
	}

	public class AddAccountCommandHandler : IRequestHandler<AddAccountCommand, ResponseMethod<AccountModel>>
	{
		private readonly IMediator _mediator;

		public AddAccountCommandHandler(IMediator mediator)
        {
			_mediator = mediator;
		}
        public async Task<ResponseMethod<AccountModel>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			return await _mediator.Send(new AuthRegisterCommand { Email = request.Email, Password = request.Password }).ConfigureAwait(false);
		}
	}
}
