using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/auth")]
    [ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository authRepository;

		public AuthController(IAuthRepository authRepository)
        {
			this.authRepository = authRepository;
		}

		[HttpPost("sign_in")]
		public async Task<IActionResult> AuthLogIn([FromBody]AddUserRequestDto user)
		{
			var token = await this.authRepository.LogIn(user);
			if (string.IsNullOrEmpty(token)) return BadRequest(new
			{
				status = false,
				message = "Email or password isn't correct"
			});
			return Ok(new
			{
				status = true,
				message = "Login successfully",
				token
			});

		}

		[HttpPost("sign_up")]
		public async Task<IActionResult> AuthRegister([FromBody] AddUserRequestDto user)
		{
			if (await this.authRepository.Register(user)) return Ok(new
			{
				status = true,
				message = "Register successfully"
			});
			return BadRequest(new
			{
				status = false,
				message = "Account is existing"
			});
		}

		[HttpGet("active")]
		public async Task<IActionResult> AuthActive(string token)
		{
			if (await this.authRepository.Active(token)) return Ok(new
			{
				status = true,
				message = "Active your account successfully"
			});
			return BadRequest(new
			{
				status = false,
				message = "Active account fail"
			});
		}

		[HttpPut("update_password")]
		public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDto updatePasswordRequestDto)
		{
			var result = await this.authRepository.UpdatePassword(updatePasswordRequestDto);
			if (result)
			{
				return Ok(new
				{
					status = true,
					message = "Password Updated Successfully"
				});
			} else
			{
				return BadRequest(new
				{
					status = false,
					message = "Unable to update password."
				});
			}
		}

		[HttpPost("forgot_password")]
		public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordRequestDto forgotPasswordRequestDto)
		{
			var result = await this.authRepository.GenerateNewPassword(forgotPasswordRequestDto.Email);
			if (result == false)
			{
				return BadRequest(new
				{
					status = false,
					message = "Unable to update password"
				});
			} else
			{
				return Ok(new
				{
					status = true,
					message = "Password successfully generated, please check your email"
				});
			}
		}

	}
}
