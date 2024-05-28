namespace ClassroomManagerAPI.Services.IServices
{
    public interface IJobService
    {
        public void ReccuringJobHourly();
        public void RecurringJobDaily();
        public void RemoveRecurringAllJob();
    }
}
