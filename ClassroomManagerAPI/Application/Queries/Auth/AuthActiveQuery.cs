using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
using MediatR;

namespace ClassroomManagerAPI.Application.Queries.Auth
{
    public class AuthActiveQuery : IRequest<ResponseMethod<bool>>
	{
        public string token { get; set; }
    }

	public class ActiveQueryHandler : IRequestHandler<AuthActiveQuery, ResponseMethod<bool>>
	{
		private readonly ITokenService _tokenService;
		private readonly IAuthRepository _authRepository;

		public ActiveQueryHandler(ITokenService tokenService, IAuthRepository authRepository)
        {
			_tokenService = tokenService;
			_authRepository = authRepository;
		}
        public async Task<ResponseMethod<bool>> Handle(AuthActiveQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<bool> response = new ResponseMethod<bool>();
			var email = _tokenService.decodeToken(request.token);
			if (!await _authRepository.Active(email))
			{
				response.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
				response.StatusCode = StatusCodes.Status404NotFound;
				return response;
			}
			response.Data = true;
			response.StatusCode = StatusCodes.Status200OK; 
			return response;
		}
	}
}
