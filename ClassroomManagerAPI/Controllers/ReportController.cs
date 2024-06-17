using ClassroomManagerAPI.Application.Commands.Facility;
using ClassroomManagerAPI.Application.Commands.Report;
using ClassroomManagerAPI.Application.Queries.Facility;
using ClassroomManagerAPI.Application.Queries.Report;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Models.Report;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
	[ApiVersion(Settings.APIVersion)]
	[Route(Settings.APIDefaultRoute + "/report")]
	[ApiController]
	public class ReportController : ControllerBase
	{
		private readonly ILogger<ReportController> _logger;
		private readonly IMediator _mediator;

		public ReportController(ILogger<ReportController> logger, IMediator mediator)
        {
			_logger = logger;
			_mediator = mediator;
		}

		//Get all
		[HttpGet]
		[ProducesResponseType(typeof(ResponseMethod<IEnumerable<ReportModel>>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetAll([FromQuery] GetAllReportsQuery query)
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

		//get by id
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ResponseMethod<ReportModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new GetByReportIdQuery { Id = id }).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		//Get by classroom address
		[HttpGet("search")]
		[ProducesResponseType(typeof(ResponseMethod<IEnumerable<ReportModel>>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetByName([FromQuery] SearchReportQuery search)
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

		//Create
		[HttpPost]
        [ProducesResponseType(typeof(ResponseMethod<ReportModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Create([FromBody] AddReportCommand command)
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

		//Update
		[HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseMethod<ReportModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReportCommand command)
		{
			try
			{
				command.Id = id;
				var result = await _mediator.Send(command).ConfigureAwait(false);
				return result.GetResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		//Delete
		[HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseMethod<ReportModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var result = await _mediator.Send(new DeleteReportCommand { Id = id }).ConfigureAwait(false);
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
