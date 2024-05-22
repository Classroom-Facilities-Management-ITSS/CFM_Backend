using ClassroomManagerAPI.Application.Queries;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models.User;
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
        [ProducesResponseType(typeof(ResponseMethod<UserModel>), (int)HttpStatusCode.OK)]
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
    }
}
