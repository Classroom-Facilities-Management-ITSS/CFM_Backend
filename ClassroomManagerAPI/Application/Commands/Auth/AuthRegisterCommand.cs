using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Configs.Mappers;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Auth;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace ClassroomManagerAPI.Application.Commands.Auth
{
	public class AuthRegisterCommand : RegisterModel, IRequest<Response<string>>
	{

	}

	public class RegisterCommandHandler : IRequestHandler<AuthRegisterCommand, Response<string>>
	{
		private readonly IAuthRepository _authRepository;
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;
		private readonly IBCryptService _bCryptService;
		private readonly ITokenService _tokenService;
		private readonly IMailService _mailService;

		public RegisterCommandHandler(IAuthRepository authRepository, 
			IMapper mapper, 
			IUserRepository userRepository, 
			IBCryptService bCryptService, 
			ITokenService tokenService, 
			IMailService mailService)
        {
			_authRepository = authRepository;
			_mapper = mapper;
			_userRepository = userRepository;
			_bCryptService = bCryptService;
			_tokenService = tokenService;
			_mailService = mailService;
		}
        public async Task<Response<string>> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			Response<string> result = new Response<string>();
			var newModel = _mapper.Map<Entities.Account>(request);
			newModel.Password = _bCryptService.HashPassword(request.Password);
			
			var createdModel = await _authRepository.Register(newModel).ConfigureAwait(false);
			if (createdModel == Guid.Empty)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataAlreadyExist));
				result.StatusCode = StatusCodes.Status403Forbidden;
				return result;
			}
			await _userRepository.AddAsync(new Entities.UserInfo { AccountId = createdModel});
			var authClaims = new List<Claim>()
				{
					new Claim(ClaimTypes.Email, newModel.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};
			var token = _tokenService.generateToken(authClaims);
			var pathHtml = Path.Combine(Directory.GetCurrentDirectory(), Settings.ResourcesVerify);
			string htmlContent = File.ReadAllText(pathHtml);
			string replacedHtmlContent = htmlContent
			.Replace("{{password}}", request.Password)
			.Replace("{{token}}", token)
			.Replace("{{email}}", newModel.Email);
			MailRequest mailRequest = new MailRequest();
			mailRequest.toEmail = newModel.Email;
			mailRequest.body = replacedHtmlContent;
			mailRequest.subject = "Verify Account";
            await _mailService.SendMail(mailRequest);
            result.Data = "Account created successfully!";
			result.StatusCode = StatusCodes.Status201Created;		
			return result;
		}
	}
}
