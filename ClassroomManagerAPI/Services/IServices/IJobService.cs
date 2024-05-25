namespace ClassroomManagerAPI.Services.IServices
{
    public interface IJobService
    {
        public Task ReccuringJob(string email);
        public void RemoveRecurringAllJob();
    }
}
