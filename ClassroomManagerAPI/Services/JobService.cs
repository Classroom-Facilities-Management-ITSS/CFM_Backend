using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services.IServices;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;

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
        public async Task CheckUpdateStatusClass()
        {
            var classroom = _classroomRepostitory.Queryable().Where(x => !x.IsDeleted && x.Status != ClassroomStatusEnum.FIXING);
            var now = DateTime.Now.Hour;
            foreach (var item in classroom) { 
                if(now >= 19 || now <= 6)
                {
                    item.Status = ClassroomStatusEnum.CLOSED;
                }else
                {
                    var exsting = await _scheduleRepository.Queryable().FirstOrDefaultAsync(x => x.StartTime.Date == DateTime.Now && x.StartTime.Hour >= now && now <= x.EndTime.Hour && x.ClassroomId == item.Id);
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
            _recurringJob.AddOrUpdate("UpdateClass", () =>  CheckUpdateStatusClass(), Cron.Hourly);
        }

        public void RecurringJobDaily()
        {
            _recurringJob.AddOrUpdate(Guid.NewGuid().ToString(), () => Console.WriteLine("Hello"), Cron.Minutely);
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
