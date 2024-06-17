using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Helpers;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ClassroomManagerAPI.Services
{
    public class JobService : IJobService
    {
        private readonly IMailService _mailService;
        private readonly IRecurringJobManager _recurringJob;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassroomRepository _classroomRepostitory;

        public JobService(IMailService mailService, 
            IRecurringJobManager recurringJob, IScheduleRepository scheduleRepository, IClassroomRepository classroomRepository)
        {
            _mailService = mailService;
            _recurringJob = recurringJob;
            _scheduleRepository = scheduleRepository;
            _classroomRepostitory = classroomRepository;
        }
        public async Task NotifiSchedule()
        {
            var now = DateTime.Now;
            var schedules = _scheduleRepository.Queryable.Include(x => x.Account).Include(x => x.Classroom).Where(x => !x.IsDeleted && x.StartTime.Value.Date == now.Date).GroupBy(x => x.Account.Email).ToList();
            foreach(var schedule in schedules)
            {
                var pathHtml = Path.Combine(Directory.GetCurrentDirectory(), Settings.ResourecesNotification);
                string htmlContent = File.ReadAllText(pathHtml);
                var body = string.Empty;
                foreach(var item in schedule)
                {
                    body += string.Format(CultureInfo.InvariantCulture, Settings.NotificationBody, item?.Subject, item?.Classroom?.Address ,item?.StartTime.Value.GetTime(), item?.EndTime.Value.GetTime());
                }
                string replacedHtmlContent = htmlContent.Replace("{{body}}", body).Replace("{{email}}", schedule.Key);
                await _mailService.SendMail(new Common.MailRequest
                {
                    body = replacedHtmlContent,
                    subject = "Class Notification",
                    toEmail = schedule.Key
                });
            }
        }
        public async Task CheckUpdateStatusClass()
        {
            var classroom = _classroomRepostitory.Queryable.Where(x => !x.IsDeleted && x.Status != ClassroomStatusEnum.FIXING).ToList();
            var now = DateTime.Now.Hour;
            foreach (var item in classroom) { 
                if(now >= 18 || now <= 6)
                {
                    var lastUsed = _scheduleRepository.Queryable.FirstOrDefault(x => !x.IsDeleted && x.ClassroomId == item.Id && x.EndTime.Value.Date == DateTime.Now && x.EndTime.Value.Hour <= now);
                    if(lastUsed != null)
                    {
                        item.LastUsed = lastUsed.EndTime.Value;
                    }
                    item.Status = ClassroomStatusEnum.CLOSED;
                }else
                {
                    var exsting = await _scheduleRepository.Queryable.FirstOrDefaultAsync(x => x.StartTime.Value.Date == DateTime.Now && x.StartTime.Value.Hour >= now && now <= x.EndTime.Value.Hour && x.ClassroomId == item.Id);
                    if(exsting != null)
                    {
                        item.Status = ClassroomStatusEnum.STUDYING;
                    }else item.Status = ClassroomStatusEnum.OPEN;
                }
                await _classroomRepostitory.UpdateAsync(item).ConfigureAwait(false);
            }
            Console.WriteLine(nameof(EnumHangFireSystem.CheckHourlyHangFireServer));
        }

        public void ReccuringJobHourly()
        {
            _recurringJob.AddOrUpdate("UpdateClass", () => CheckUpdateStatusClass(), "*/30 * * * *", TimeZoneInfo.Local);
        }

        public void RecurringJobDaily()
        {
            _recurringJob.AddOrUpdate("NotificationClass", () => NotifiSchedule(), "0 6 * * *", TimeZoneInfo.Local);
        }
        public void RemoveRecurringAllJob()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var reccurring in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(reccurring.Id);
                }
            }
        }
    }
}
