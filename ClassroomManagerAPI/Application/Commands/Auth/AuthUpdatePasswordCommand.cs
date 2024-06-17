using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Auth;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
using MediatR;

namespace ClassroomManagerAPI.Application.Commands.Auth
{
    public class AuthUpdatePasswordCommand : UpdateModel, IRequest<ResponseMethod<string>>
	{

	}

	public class UpdatePasswordCommandHandler : IRequestHandler<AuthUpdatePasswordCommand, ResponseMethod<string>>
	{
		private readonly IAuthRepository _authRepository;
		private readonly AuthContext _authContext;
		private readonly IBCryptService _bCryptService;

		public UpdatePasswordCommandHandler(IAuthRepository authRepository, AuthContext authContext, IBCryptService bCryptService)
		{
			_authRepository = authRepository;
			_authContext = authContext;
			_bCryptService = bCryptService;
		}
		public async Task<ResponseMethod<string>> Handle(AuthUpdatePasswordCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> response = new ResponseMethod<string>();
			if (string.IsNullOrEmpty(request.Email))
			{
				request.Email = _authContext.GetCurrentEmail();
				if (string.IsNullOrEmpty(request.Email))
				{
					response.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
					response.StatusCode = StatusCodes.Status404NotFound;
					return response;
				}
			}
			var found = await _authRepository.GetByEmailAsync(request.Email).ConfigureAwait(false);
			if (found == null)
			{
				response.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
				response.StatusCode = StatusCodes.Status404NotFound;
				return response;
			}
			if (request.OldPassword == request.NewPassword)
			{
				response.AddBadRequest(nameof(ErrorAuthEnum.OldPasswordMatching));
				response.StatusCode = StatusCodes.Status403Forbidden;
				return response;
			}
			if (!_bCryptService.verifyPassword(request.OldPassword, found.Password))
			{
				response.AddBadRequest(nameof(ErrorAuthEnum.InvalidPassword));
				response.StatusCode = StatusCodes.Status403Forbidden;
				return response;
			}
			found.Password = _bCryptService.HashPassword(request.NewPassword);
			await _authRepository.UpdateAsync(found).ConfigureAwait(false);
			response.Data = "Password updated successfully!";
			response.StatusCode = StatusCodes.Status200OK;
			return response;
		}
	}
}
