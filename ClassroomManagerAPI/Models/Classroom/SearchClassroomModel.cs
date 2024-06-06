using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Models.Classroom
{
	public class SearchClassroomModel : FilterModel
	{
		public string? ClassroomAddress { get; set; }
		public DateTime? LastUsed { get; set; }
		public int? FacilityAmount { get; set; }
		public int? MaxSize { get; set; }
		public string? Note { get; set; }
		public ClassroomStatusEnum? Status { get; set; }
	}
}
