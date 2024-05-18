using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services;
using ClassroomManagerAPI.Services.IServices;
using MediatR;

namespace ClassroomManagerAPI.Application.Commands.Auth
{
	public class AuthGeneratePasswordCommand : IRequest<Response<bool>>
	{
        public string Email { get; set; }
    }

	public class AuthGeneratePasswordCommandHandler : IRequestHandler<AuthGeneratePasswordCommand, Response<bool>>
	{
		private readonly IConfiguration _configuration;
		private readonly IAuthRepository _authRepository;
		private readonly IBCryptService _bCryptService;
		private readonly IMailService _mailService;

		public AuthGeneratePasswordCommandHandler(IConfiguration configuration, IAuthRepository authRepository, IBCryptService bCryptService, IMailService mailService)
        {
			_configuration = configuration;
			_authRepository = authRepository;
			_bCryptService = bCryptService;
			_mailService = mailService;
		}
        public async Task<Response<bool>> Handle(AuthGeneratePasswordCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			var response = new Response<bool>();
			var chars = _configuration["resetKey"];
			var random = new Random();
			var passwordChars = new char[8];

			for (int i = 0; i < passwordChars.Length; i++)
			{
				passwordChars[i] = chars[random.Next(chars.Length)];
			}

			string passwordString = new string(passwordChars);

			var found = await _authRepository.GetByEmailAsync(request.Email).ConfigureAwait(false);
			if (found == null)
			{
				response.AddBadRequest(nameof(ErrorAuthEnum.AccountNotExist));
				response.StatusCode = StatusCodes.Status404NotFound;
				return response;
			}

			var password = _bCryptService.HashPassword(passwordString);
			found.Password = password;
			await _authRepository.UpdateAsync(found).ConfigureAwait(false);

			var mailRequest = new MailRequest
			{
				toEmail = request.Email,
				subject = "Your new Password",
				body = $"Use this password to login and change your password: {passwordString} "
			};

			await this._mailService.SendMail(mailRequest);

			response.Data = true;
			response.StatusCode = StatusCodes.Status200OK;
			return response;
		}
	}
}
