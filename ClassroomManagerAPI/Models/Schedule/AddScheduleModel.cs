using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Models.Schedule
{
	public class AddScheduleModel 
	{
		public Guid ClassroomId { get; set; }
		public Guid AccountId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public ScheduleStatusEnum Status { get; set; }
		public int CountStudent { get; set; }
		public string Subject { get; set; }
	}
}
