using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Models.Auth;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services;
using ClassroomManagerAPI.Services.IServices;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClassroomManagerAPI.Application.Commands.Auth
{
    public class AuthLoginCommand : LoginModel, IRequest<ResponseMethod<AuthModel>>
	{
	}

	public class LoginCommandHandler : IRequestHandler<AuthLoginCommand, ResponseMethod<AuthModel>>
	{
		private readonly IMapper _mapper;
		private readonly IAuthRepository _authRepository;
		private readonly IBCryptService _bCryptService;
		private readonly ITokenService _tokenService;

		public LoginCommandHandler(IMapper mapper, 
			IAuthRepository authRepository,
			IBCryptService bCryptService,
			ITokenService tokenService)
        {
			_mapper = mapper;
			_authRepository = authRepository;
			_bCryptService = bCryptService;
			_tokenService = tokenService;
		}
        public async Task<ResponseMethod<AuthModel>> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<AuthModel> result = new ResponseMethod<AuthModel>();
			var newModel = _mapper.Map<Entities.Account>(request);

			var loginModel = await _authRepository.LogIn(newModel).ConfigureAwait(false);
			if (loginModel == null)
			{
				result.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
				result.StatusCode = StatusCodes.Status404NotFound;
				return result;
			}

			if (!loginModel.Active)
			{
				result.AddBadRequest(nameof(ErrorAuthEnum.AccountNotActive));
				result.StatusCode = StatusCodes.Status403Forbidden;
				return result;
			}

			if (!_bCryptService.verifyPassword(request.Password, loginModel.Password))
			{
				result.AddBadRequest(nameof(ErrorAuthEnum.InvalidPassword));
				result.StatusCode = StatusCodes.Status403Forbidden;
				return result;
			}

			var authClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, loginModel.Id.ToString()),
				new Claim(ClaimTypes.Email, newModel.Email),
				new Claim(ClaimTypes.Role, loginModel.Role.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};
		
			result.Data = new AuthModel
			{
				AccessToken = _tokenService.generateToken(authClaims),
				Email = request.Email,
				Expiration = DateTime.Now.AddDays(30),
				Role = loginModel.Role.ToString(),
			};
			result.StatusCode = StatusCodes.Status200OK; 
			return result;
		}
	}
}
