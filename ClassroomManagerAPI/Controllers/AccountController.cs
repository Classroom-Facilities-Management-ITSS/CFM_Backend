using ClassroomManagerAPI.Application.Commands.Account;
using ClassroomManagerAPI.Application.Commands.Facility;
using ClassroomManagerAPI.Application.Queries.Account;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Facility;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
	[ApiVersion(Settings.APIVersion)]
	[Route(Settings.APIDefaultRoute + "/accounts")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<AccountController> _logger;

		public AccountController(ILogger<AccountController> logger, IMediator mediator)
		{
			_mediator = mediator;
			_logger = logger;
		}

		//Get all accounts
		[HttpGet]
		[ProducesResponseType(typeof(Response<IEnumerable<AccountModel>>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllAccountQuery query)
		{
			try
			{
				var result = await _mediator.Send(query).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Create an account
		[HttpPost]
		[ProducesResponseType(typeof(Response<AccountModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Create([FromBody] AddAccountCommand command)
		{
			try
			{
				var result = await _mediator.Send(command).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Update an account
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(Response<AccountModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountCommand command)
		{
			try
			{
				command.Id = id;
				// Map Dto to domain models
				var result = await _mediator.Send(command).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Remove an account
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(Response<AccountModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new DeleteAccountCommand { Id = id }).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}
	}
}
