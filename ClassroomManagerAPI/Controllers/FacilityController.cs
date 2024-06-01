using ClassroomManagerAPI.Application.Commands.Facility;
using ClassroomManagerAPI.Application.Queries.Facility;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Facility;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/facility")]
	[ApiController]
	public class FacilityController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<FacilityController> _logger;

		public FacilityController(ILogger<FacilityController> logger, IMediator mediator)
		{
			_mediator = mediator;
			_logger = logger;
		}

		// Get all
		[HttpGet]
		[ProducesResponseType(typeof(ResponseMethod<IEnumerable<FacilityModel>>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllFacilityQuery query)
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
        [ProducesResponseType(typeof(ResponseMethod<FacilityModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new GetByFacilityIdQuery { Id = id }).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Get by name
		[HttpGet("search")]
        [ProducesResponseType(typeof(ResponseMethod<IEnumerable<FacilityModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByName([FromQuery] SearchFacilityQuery search)
		{
			try
			{
				var result = await _mediator.Send(search).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}


		// Create facility
		[HttpPost]
        [ProducesResponseType(typeof(ResponseMethod<FacilityModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AddFacilityCommand command)
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

		// Update facility
		[HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseMethod<FacilityModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFacilityCommand command)
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

		// Remove a facility
		[HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseMethod<FacilityModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new DeleteFacilityCommand { Id = id }).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		[HttpGet("export")]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> ExportReportFacility([FromQuery] ExportFacilityQuery query)
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
    }
}
