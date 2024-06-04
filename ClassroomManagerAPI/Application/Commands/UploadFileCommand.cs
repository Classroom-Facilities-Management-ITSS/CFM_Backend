using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Helpers;
using MediatR;

namespace ClassroomManagerAPI.Application.Commands
{
    public class UploadFileCommand : IRequest<ResponseMethod<string>>
    {
        public IFormFile File { get; set; }
    }

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, ResponseMethod<string>>
    {
        public async Task<ResponseMethod<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<string>();
            var newFile = FileHelper.saveFile(request.File);
            if (string.IsNullOrEmpty(newFile))
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.ExportFaile));
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;
            }
            result.Data = newFile;
            result.StatusCode = StatusCodes.Status200OK;
            return result;
        }
    }
}
