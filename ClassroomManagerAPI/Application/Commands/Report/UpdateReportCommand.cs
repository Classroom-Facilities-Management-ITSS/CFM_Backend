using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Models.Report;
using ClassroomManagerAPI.Repositories.IRepositories;
using MediatR;
using System.Net;

namespace ClassroomManagerAPI.Application.Commands.Report
{
    public class UpdateReportCommand : UpdateReportModel, IRequest<ResponseMethod<string>>
	{
	}

	public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, ResponseMethod<string>>
	{
		private readonly IReportRepository _reportRepository;
		private readonly IMapper _mapper;
		public UpdateReportCommandHandler(IMapper mapper, IReportRepository reportRepository)
		{
			_reportRepository = reportRepository;
			_mapper = mapper;
		}
		public async Task<ResponseMethod<string>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			var updatedFacility = await _reportRepository.UpdateAsync( _mapper.Map<Entities.Report>(request)).ConfigureAwait(false);
			if (updatedFacility == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"update facility with id {request.Id} sucessfully";
			return result;
		}
	}
}
