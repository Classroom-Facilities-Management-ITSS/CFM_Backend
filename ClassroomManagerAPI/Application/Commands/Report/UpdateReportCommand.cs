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
	public class UpdateReportCommand : UpdateReportModel, IRequest<ResponseMethod<ReportModel>>
	{
	}

	public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, ResponseMethod<ReportModel>>
	{
		private readonly IReportRepository _reportRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IClassroomRepository _classroomRepository;
		private readonly IMapper _mapper;
		private readonly AuthContext _authContext;

		public UpdateReportCommandHandler(IMapper mapper, IReportRepository reportRepository, IAccountRepository accountRepository, IClassroomRepository classroomRepository, AuthContext authContext)
		{
			_reportRepository = reportRepository;
			_accountRepository = accountRepository;
			_classroomRepository = classroomRepository;
			_mapper = mapper;
			_authContext = authContext;
		}

		public async Task<ResponseMethod<ReportModel>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<ReportModel> result = new ResponseMethod<ReportModel>();

			if (request.AccountId == null)
			{
				request.AccountId = _authContext.GetCurrentId();
			}

			var account = await _accountRepository.GetByIDAsync(request.AccountId.Value);
			if (account == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}

			if (request.ClassroomId.HasValue)
			{
				var classroom = await _classroomRepository.GetByIDAsync(request.ClassroomId.Value);
				if (classroom == null)
				{
					result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
					result.StatusCode = (int)HttpStatusCode.NotFound;
					return result;
				}
			}

			var updatedReport = await _reportRepository.UpdateAsync(_mapper.Map<Entities.Report>(request)).ConfigureAwait(false);
			if (updatedReport == null)
			{
				result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
				result.StatusCode = (int)HttpStatusCode.NotFound;
				return result;
			}
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = _mapper.Map<ReportModel>(updatedReport);
			return result;
		}
	}
}
