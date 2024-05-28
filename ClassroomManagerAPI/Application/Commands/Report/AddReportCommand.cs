using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Report
{
    public class AddReportCommand : AddReportModel, IRequest<ResponseMethod<ReportModel>>
	{
	}

	public class AddReportCommandHandler : IRequestHandler<AddReportCommand, ResponseMethod<ReportModel>>
	{
		private readonly IReportRepository _reportRepository;
		private readonly IMapper _mapper;
		public AddReportCommandHandler(IMapper mapper, IReportRepository reportRepository)
		{
			_reportRepository = reportRepository;
			_mapper = mapper;
		}

		public async Task<ResponseMethod<ReportModel>> Handle(AddReportCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ReportModel> result = new ResponseMethod<ReportModel>();
			var createdReport = await _reportRepository.AddAsync(_mapper.Map<Entities.Report>(request)).ConfigureAwait(false);
			result.StatusCode = (int)HttpStatusCode.Created;
			result.Data = _mapper.Map<ReportModel>(createdReport);
			return result;
		}
	}
}
