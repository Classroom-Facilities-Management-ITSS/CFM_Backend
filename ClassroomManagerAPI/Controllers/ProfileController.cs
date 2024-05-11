﻿using ClassroomManagerAPI.Application.Queries;
using ClassroomManagerAPI.Configs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> getFileSystem()
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
