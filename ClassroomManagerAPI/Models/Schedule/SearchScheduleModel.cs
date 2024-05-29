namespace ClassroomManagerAPI.Models.Schedule
{
	public class SearchScheduleModel : FilterModel
	{
		public string? ClassroomAddress { get; set; }
		public string? Email { get; set; }
		public string? FullName { get; set; }
		public DateOnly? Date { get; set; }
		public TimeOnly? StartTime { get; set; }
		public TimeOnly? EndTime { get; set; }
		public int? CountStudent { get; set; }
		public string? Subject { get; set; }
	}
}
