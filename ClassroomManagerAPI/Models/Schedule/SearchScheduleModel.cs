namespace ClassroomManagerAPI.Models.Schedule
{
	public class SearchScheduleModel : FilterModel
	{
		public string? ClassroomAddress { get; set; }
		public string? Email { get; set; }
		public string? UserName { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public int? CountStudent { get; set; }
		public string? Subject { get; set; }
	}
}
