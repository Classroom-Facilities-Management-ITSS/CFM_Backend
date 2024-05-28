using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Report
{
    public class DeleteReportCommand : IRequest<ResponseMethod<string>>
	{
		public Guid Id { get; set; }
	}

	public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, ResponseMethod<string>>
	{
		private readonly IReportRepository _reportRepository;
		public DeleteReportCommandHandler(IReportRepository reportRepository)
		{
			_reportRepository = reportRepository;
		}
		public async Task<ResponseMethod<string>> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var deletedFacility = await _reportRepository.DeleteAsync(request.Id).ConfigureAwait(false);
			if (!deletedFacility)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"Delete facility with id {request.Id} sucessfully";
			return result;
		}
	}
}
