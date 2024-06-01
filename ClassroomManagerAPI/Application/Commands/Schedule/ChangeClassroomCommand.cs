using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Models.Schedule;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace ClassroomManagerAPI.Application.Commands.Schedule
{
	public class ChangeClassroomCommand : ChangeClassroomModel, IRequest<ResponseMethod<string>>
	{
    }

	public class ChangeClassroomCommandHandler : IRequestHandler<ChangeClassroomCommand, ResponseMethod<string>>
	{
		private readonly IMapper _mapper;
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IMailService _mailService;

		public ChangeClassroomCommandHandler(IMapper mapper, IScheduleRepository scheduleRepository, IMailService mailService)
        {
			_mapper = mapper;
			_scheduleRepository = scheduleRepository;
			_mailService = mailService;
		}
        public async Task<ResponseMethod<string>> Handle(ChangeClassroomCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);
			ResponseMethod<string> result = new ResponseMethod<string>();
			// go to schedule, change the classroom in the schedule
			var schedule = _mapper.Map<Entities.Schedule>(request);
			var updatedSchedule = await _scheduleRepository.UpdateAsync(schedule).ConfigureAwait(false);
			// send email about the schedule to user
			var authClaims = new List<Claim>()
				{
					new Claim(ClaimTypes.Email, updatedSchedule.Account.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};
			var pathHtml = Path.Combine(Directory.GetCurrentDirectory(), Settings.ResourcesVerify);
			string htmlContent = File.ReadAllText(pathHtml);
			MailRequest mailRequest = new MailRequest();
			mailRequest.toEmail = updatedSchedule.Account.Email;
			mailRequest.body = htmlContent;
			mailRequest.subject = "Update schedule's classroom";
			await _mailService.SendMail(mailRequest);
			result.StatusCode = (int)HttpStatusCode.OK;
			result.Data = $"update classroom sucessfully";
			return result;
		}
	}
}
