using ClassroomManagerAPI.Application.Commands.Facility;
using ClassroomManagerAPI.Application.Commands.Schedule;
using ClassroomManagerAPI.Application.Queries.Facility;
using ClassroomManagerAPI.Application.Queries.Schedule;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Models.Schedule;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
	[ApiVersion(Settings.APIVersion)]
	[Route(Settings.APIDefaultRoute + "/schedule")]
	[ApiController]
	public class ScheduleController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<ScheduleController> _logger;

		public ScheduleController(ILogger<ScheduleController> logger, IMediator mediator)
		{
			_mediator = mediator;
			_logger = logger;
		}
		// Get all
		[HttpGet]
		[ProducesResponseType(typeof(ResponseMethod<IEnumerable<ScheduleModel>>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllScheduleQuery query)
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

		// Get by id
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ResponseMethod<ScheduleModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new GetScheduleByIdQuery { Id = id }).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}
		// Search


		// Create 
		[HttpPost]
		[ProducesResponseType(typeof(ResponseMethod<ScheduleModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Create([FromBody] AddScheduleCommand command)
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
		// Update 
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ResponseMethod<ScheduleModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateScheduleCommand command)
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
		// Remove
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ResponseMethod<ScheduleModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new DeleteScheduleCommand { Id = id }).ConfigureAwait(false);
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
