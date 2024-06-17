using ClassroomManagerAPI.Application.Commands;
using ClassroomManagerAPI.Application.Queries;
using ClassroomManagerAPI.Application.Queries.Report;
using ClassroomManagerAPI.Application.Queries.Schedule;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Schedule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(ILogger<ProfileController> logger, IMediator mediator) {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseMethod<AccountModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> getMyProfile()
        {
            try
            {
                var result = await _mediator.Send(new GetProfileQuery { }).ConfigureAwait(false);
                return result.GetResult();
            }catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ResponseMethod<AccountModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateInfor([FromBody] UpdateProfileCommand command)
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

        [HttpGet("schedule")]
        [ProducesResponseType(typeof(ResponseMethod<IEnumerable<ScheduleModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMySchedules([FromQuery] GetMyScheduleQuery query)
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

        [HttpGet("report")]
        [ProducesResponseType(typeof(ResponseMethod<IEnumerable<ScheduleModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMyReports([FromQuery] GetMyReportQuery query)
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
