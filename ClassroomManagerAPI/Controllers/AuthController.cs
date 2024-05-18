using ClassroomManagerAPI.Application.Commands.Auth;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Auth;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/auth")]
    [ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
        {
			_mediator = mediator;
		}

		
		[HttpPost("sign_in")]
		[ProducesResponseType(typeof(Response<AuthModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> AuthLogIn([FromBody] AuthLoginCommand command)
		{
			var result = await _mediator.Send(command).ConfigureAwait(false);
			return result.GetResult();

		}
		
		[HttpPost("sign_up")]
		[ProducesResponseType(typeof(Response<RegisterModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> AuthRegister([FromBody] AuthRegisterCommand command)
		{
			var result = await _mediator.Send(command).ConfigureAwait(false);

			return result.GetResult();
		}

		/*
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
		*/
	}
}
