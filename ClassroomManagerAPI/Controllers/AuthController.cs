﻿using ClassroomManagerAPI.Application.Commands.Auth;
using ClassroomManagerAPI.Application.Queries.Auth;
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


		[HttpGet("active")]
		[ProducesResponseType(typeof(Response<RegisterModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> AuthActive([FromQuery] AuthActiveQuery query)
		{
			var result = await _mediator.Send(query).ConfigureAwait(false);

			return result.GetResult();
		}

		

		[HttpPut("update_password")]
		[ProducesResponseType(typeof(Response<AuthModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> UpdatePassword([FromBody] AuthUpdatePasswordCommand command)
		{
			var result = await _mediator.Send(command).ConfigureAwait(false);

			return result.GetResult();
		}

		[HttpPost("forgot_password")]
		[ProducesResponseType(typeof(Response<AuthModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> ForgotPassword([FromBody] AuthGeneratePasswordCommand command)
		{
			var result = await _mediator.Send(command).ConfigureAwait(false);

			return result.GetResult();
		}
	}
}
