using ClassroomManagerAPI.Application.Commands;
using ClassroomManagerAPI.Application.Commands.Schedule;
using ClassroomManagerAPI.Application.Queries;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute)]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseMethod<Stream>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetImageFormServer([FromQuery] GetFileQuery query)
        {
            var result = await _mediator.Send(query).ConfigureAwait(false);
            if(!result.IsOk || result.Data == null) return result.GetResult();
            return File(result.Data , Settings.ImageFile);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseMethod<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> PostImageToServer([FromForm] UploadFileCommand command)
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return result.GetResult();
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ResponseMethod<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DelêtImageFromServer([FromQuery] DeleteFileCommand command)
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return result.GetResult();
        }

        [HttpPost("schedule")]
        [ProducesResponseType(typeof(ResponseMethod<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> PostSchedule([FromForm] UploadScheduleCommand command)
        {
            ResponseMethod<Stream> commandResult = await _mediator.Send(command).ConfigureAwait(false);
            if (!commandResult.IsOk)
            {
                return File(commandResult.Data, Settings.Excels.ContentType, "Schedule_import.xlsx");
            }
            return commandResult.GetResult();
        }
    }
}
