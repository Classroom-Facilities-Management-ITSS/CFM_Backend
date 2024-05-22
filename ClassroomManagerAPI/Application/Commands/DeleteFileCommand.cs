using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Helpers;
using MediatR;

namespace ClassroomManagerAPI.Application.Commands
{
    public class DeleteFileCommand : IRequest<ResponseMethod<bool>>
    {
        public string name { get; set; }
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, ResponseMethod<bool>>
    {
        public async Task<ResponseMethod<bool>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<bool>();
            if (!FileHelper.removeFile(request.name))
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = StatusCodes.Status404NotFound;
                return result;
            }
            result.Data = true;
            result.StatusCode = StatusCodes.Status200OK;
            return result;
        }
    }
}
