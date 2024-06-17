using ClassroomManagerAPI.Application.Commands.Schedule;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums.ErrorCodes;
using ClassroomManagerAPI.Helpers;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Application.Commands
{
    public class SuggestChangeCommand : IRequest<ResponseMethod<bool>>
    {
        public Guid CurrentClassId { get; set; }
        public Guid ChangeClassId { get; set; }
    }

    public class SuggestChangeCommandHandler : IRequestHandler<SuggestChangeCommand, ResponseMethod<bool>>
    {
        private readonly IClassroomRepository _classRepository;
        private readonly IMailService _mailService;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAccountRepository _accountRepostiory;
        private readonly IMediator _mediator;

        public SuggestChangeCommandHandler(IClassroomRepository classroomRepository, IMailService mailService, IScheduleRepository scheduleRepository, IMediator mediator, IAccountRepository accountRepository)
        {
            _classRepository = classroomRepository;
            _mailService = mailService;
            _scheduleRepository = scheduleRepository;
            _accountRepostiory = accountRepository;
            _mediator = mediator;
        }
        public async Task<ResponseMethod<bool>> Handle(SuggestChangeCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new ResponseMethod<bool>();    
            var currentClass = _classRepository.Queryable.Any(x => x.Id == request.CurrentClassId);
            var changeClass = _classRepository.Queryable.Any(x => x.Id == request.ChangeClassId);
            if(currentClass && changeClass) {
                var schedules = _scheduleRepository.Queryable.Where(x => x.ClassroomId == request.CurrentClassId && x.StartTime.HasValue && x.StartTime.Value.Date == DateTime.Now.Date).ToList();
                foreach( var schedule in schedules )
                {
                    await _mediator.Send(new UpdateScheduleCommand
                    {
                        Id = schedule.Id,
                        Subject = schedule.Subject,
                        StartTime = schedule.StartTime,
                        CountStudent = schedule.CountStudent,
                        AccountId = schedule.AccountId,
                        EndTime = schedule.EndTime,
                        ClassroomId = request.ChangeClassId
                    }).ConfigureAwait(false);

                    var classroomName = await _classRepository.Queryable.FirstOrDefaultAsync(x => x.Id == schedule.ClassroomId, cancellationToken);
                    var mail = await _accountRepostiory.Queryable.FirstOrDefaultAsync(x => x.Id == schedule.AccountId, cancellationToken);
                    var pathHtml = Path.Combine(Directory.GetCurrentDirectory(), Settings.ResourecesChangeClass);
                    string htmlContent = File.ReadAllText(pathHtml);
                    string replacedHtmlContent = htmlContent
                        .Replace("{{subject}}", schedule.Subject)
                        .Replace("{{date}}", schedule.StartTime.Value.GetDate())
                        .Replace("{{class_room}}", classroomName.Address)
                        .Replace("{{email}}", mail.Email)
                        ;
                    await _mailService.SendMail(new MailRequest
                    {
                        body = replacedHtmlContent,
                        subject = "Class Change Notification",
                        toEmail = mail.Email
                    });
                }

                result.Data = true;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }

            result.AddBadRequest(nameof(ErrorSystemEnum.DataNotExist));
            result.StatusCode = StatusCodes.Status404NotFound;
            return result;
        }

    }
}
