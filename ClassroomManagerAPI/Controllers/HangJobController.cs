using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/job")]
    [ApiController]
    public class HangJobController : ControllerBase
    {
        private readonly IJobService _jobService;       
        
        public HangJobController(IJobService jobService)
        {
            _jobService = jobService;
        }
        [HttpGet("daily")]

        public async Task<IActionResult> HangJobDaily()
        {
            _jobService.RecurringJobDaily();
            return Ok();
        }
        [HttpGet("hourly")]
        public async Task<IActionResult> HangJobHourly()
        {
            _jobService.ReccuringJobHourly();
            return Ok();
        }
        [HttpGet("remove")]
        public async Task<IActionResult> RemoveHangJob()
        {
            _jobService.RemoveRecurringAllJob();
            return Ok();
        }
    }
}
