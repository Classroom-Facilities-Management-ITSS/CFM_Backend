using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models.Classroom;
using MediatR;
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
        public async Task<IActionResult> GetSuggestion()
        {
            return Ok();
        }
    }
}
