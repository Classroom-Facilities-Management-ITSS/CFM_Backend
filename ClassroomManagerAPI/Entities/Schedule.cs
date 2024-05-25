using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Entities
{
    public class Schedule : BaseEntity
    {
        public Classroom Classroom { get; set; }
        public Guid ClassroomId { get; set; }
        public Account Account { get; set; }
        public Guid AccountId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ScheduleStatusEnum Status { get; set; }
        public int CountStudent { get; set; }
        public string Subject { get; set; }
    }
}
