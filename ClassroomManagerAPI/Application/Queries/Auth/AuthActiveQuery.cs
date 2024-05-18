using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
using MediatR;

namespace ClassroomManagerAPI.Application.Queries.Auth
{
	public class AuthActiveQuery : IRequest<Response<bool>>
	{
        public string token { get; set; }
    }

	public class ActiveQueryHandler : IRequestHandler<AuthActiveQuery, Response<bool>>
	{
		private readonly ITokenService _tokenService;
		private readonly IAuthRepository _authRepository;

		public ActiveQueryHandler(ITokenService tokenService, IAuthRepository authRepository)
        {
			_tokenService = tokenService;
			_authRepository = authRepository;
		}
        public async Task<Response<bool>> Handle(AuthActiveQuery request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<bool> response = new Response<bool>();
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
