using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums.ErrorCodes;
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
        private readonly AuthContext _authContext;

        public UpdateReportCommandHandler(IMapper mapper, IReportRepository reportRepository, AuthContext authContext)
		{
			_reportRepository = reportRepository;
			_mapper = mapper;
			_authContext = authContext;
		}
		public async Task<ResponseMethod<string>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			if (request.AccountId == null)
			{
				request.AccountId = _authContext.GetCurrentId();
			}
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
