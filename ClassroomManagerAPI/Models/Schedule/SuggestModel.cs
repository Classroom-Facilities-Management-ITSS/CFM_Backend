namespace ClassroomManagerAPI.Models.Schedule
{
	public class SuggestModel : FilterModel
	{
		public string ClassroomAddress { get; set; }
		public Guid ClassroomId { get; set; }
	}
}
