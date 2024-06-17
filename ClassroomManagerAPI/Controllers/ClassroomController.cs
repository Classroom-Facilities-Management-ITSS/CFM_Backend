using ClassroomManagerAPI.Application.Commands.Classroom;
using ClassroomManagerAPI.Application.Queries.Classroom;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Classroom;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/classroom")]
    [ApiController]
	public class ClassroomController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<ClassroomController> _logger;

		public ClassroomController(ILogger<ClassroomController> logger, IMediator mediator)
        {
			_logger = logger;
			_mediator = mediator;
		}

		// Get all classrooms
		[HttpGet]
		[ProducesResponseType(typeof(ResponseMethod<IEnumerable<ClassroomModel>>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllClassroomQuery query)
		{
			try
			{
				//_logger.LogInformation("GetAll was invoked");
				var result = await _mediator.Send(query).ConfigureAwait(false);
				return result.GetResult();
				//_logger.LogInformation($"Finished get all request with data: {JsonSerializer.Serialize(objList)}");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Get by number
		[HttpGet("search")]
		[Authorize]
		[ProducesResponseType(typeof(ResponseMethod<ClassroomModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetByNumber([FromQuery] SearchClassroomQuery query)
		{
			try
			{
				var result = await _mediator.Send(query).ConfigureAwait(false);
				return result?.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Get by id
		[HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(ResponseMethod<ClassroomModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new GetClassroomByIdQuery { Id = id }).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Create classroom
		[HttpPost]
        [Authorize(nameof(RoleEnum.ADMIN))]
        [ProducesResponseType(typeof(ResponseMethod<ClassroomModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Create([FromBody] AddClassroomCommand addClassroomCommand)
		{
			try
			{
				var result = await _mediator.Send(addClassroomCommand).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Update a classroom
		[HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseMethod<ClassroomModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClassroomCommand updateClassroomCommand )
		{
			try
			{
				updateClassroomCommand.Id = id;
				var result = await _mediator.Send(updateClassroomCommand).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Remove a classroom
		[HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseMethod<ClassroomModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new DeleteClassroomCommand { Id = id }).ConfigureAwait(false);
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
