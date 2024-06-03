using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Helpers;
using MediatR;

namespace ClassroomManagerAPI.Application.Queries
{
    public class GetFileQuery : IRequest<ResponseMethod<Stream>>
    {
        public string name { get; set; }
    }

    public class GetFileQueryHandler : IRequestHandler<GetFileQuery, ResponseMethod<Stream>>
    {
        public async Task<ResponseMethod<Stream>> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<Stream>();
            var file = FileHelper.getFile(request.name);
            if(file == null)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = StatusCodes.Status404NotFound;
                return result;
            }
            result.Data = file;
            result.StatusCode = StatusCodes.Status200OK;
            return result;
        }
    }
}
