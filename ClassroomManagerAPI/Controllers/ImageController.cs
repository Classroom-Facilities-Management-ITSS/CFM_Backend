using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        [HttpGet("{name}")]
        public async Task<IActionResult> GetImageFormServer(string name)
        {
            var image = FileHelper.getFile(name);
            if (image == null) return NotFound();
            return File(image , "image/jpeg");
        }
    }
}
