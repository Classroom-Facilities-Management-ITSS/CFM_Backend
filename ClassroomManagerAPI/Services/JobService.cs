using ClassroomManagerAPI.Services.IServices;
using Hangfire;
using Hangfire.Storage;

namespace ClassroomManagerAPI.Services
{
    public class JobService : IJobService
    {
        private readonly IMailService _mailService;
        private readonly IBackgroundJobClient _jobClient;
        private readonly IRecurringJobManager _recurringJob;


        public JobService(IMailService mailService, IBackgroundJobClient jobClient, IRecurringJobManager recurringJob)
        {
            _mailService = mailService;
            _jobClient = jobClient;
            _recurringJob = recurringJob;
        }
        public async Task ReccuringJob(string email)
        {
            _recurringJob.AddOrUpdate(Guid.NewGuid().ToString(), () => Console.WriteLine("Hello"), Cron.Daily);
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
