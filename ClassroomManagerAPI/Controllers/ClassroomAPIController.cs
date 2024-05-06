using AutoMapper;
using ClassroomManagerAPI.Application.Commands.Classroom;
using ClassroomManagerAPI.Application.Commands.Facility;
using ClassroomManagerAPI.Application.Queries.Classroom;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Classroom;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Models.Facility;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/classroom")]
    [ApiController]
	public class ClassroomAPIController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<ClassroomAPIController> _logger;

		public ClassroomAPIController(ILogger<ClassroomAPIController> logger, IMediator mediator)
        {
			_logger = logger;
			_mediator = mediator;
		}

		// Get all classrooms
		[HttpGet]
		[ProducesResponseType(typeof(Response<IEnumerable<ClassroomModel>>), (int)HttpStatusCode.OK)]
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
			return response;
		}

		// Get by number
		[HttpGet("{classrooomNumber}")]
		[ProducesResponseType(typeof(Response<ClassroomModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetByNumber(string classroomNumber)
		{
			try
			{
				var result = await _mediator.Send(new GetByClassroomNumberQuery { ClassNumber = classroomNumber }).ConfigureAwait(false);
				return result?.GetResult();
			}
				response.Result = mapper.Map<ClassroomDto>(obj);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
			return response;
		}

		// Create classroom
		[HttpPost]
		[ProducesResponseType(typeof(Response<ClassroomModel>), (int)HttpStatusCode.OK)]
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
			return response;
		}

		// Update a classroom
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(Response<ClassroomModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFacilityCommand updateFacilityCommand )
		{
			try
			{
				updateFacilityCommand.Id = id;
				var result = await _mediator.Send(updateFacilityCommand).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
			return response;
		}

		// Remove a classroom
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(Response<ClassroomModel>), (int)HttpStatusCode.OK)]
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
			return response;
		}
	}
}
