using ClassroomManagerAPI.Application.Commands;
using ClassroomManagerAPI.Application.Queries;
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
    [Route(Settings.APIDefaultRoute + "/suggest")]
    [ApiController]
    public class SuggestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SuggestController> _logger;

        public SuggestController(ILogger<SuggestController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseMethod<IEnumerable<ClassroomModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetSuggestion([FromQuery] GetSuggestClassQuery query)
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

        [HttpPost]
        [ProducesResponseType(typeof(ResponseMethod<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ChangeSuggestion([FromBody] SuggestChangeCommand command)
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
    }
}
