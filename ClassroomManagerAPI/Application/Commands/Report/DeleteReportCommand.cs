using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Report
{
    public class DeleteReportCommand : IRequest<ResponseMethod<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, ResponseMethod<bool>>
    {
        private readonly IReportRepository _reportRepository;
        public DeleteReportCommandHandler(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<ResponseMethod<bool>> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ResponseMethod<bool> result = new ResponseMethod<bool>();
            var deletedFacility = await _reportRepository.DeleteAsync(request.Id).ConfigureAwait(false);
            if (!deletedFacility)
            {
                result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
                result.StatusCode = (int)HttpStatusCode.NotFound;
                result.Data = false;
                return result;
            }
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Data = true;
            return result;
        }
    }
}