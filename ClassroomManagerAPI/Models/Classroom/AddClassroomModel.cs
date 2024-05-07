using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Classroom
{
	public class AddClassroomModel
	{
		[Required]
		public string ClassNumber { get; set; }
		public string? LastUsed { get; set; }
		public int FacilityAmount { get; set; }
		public string? Note { get; set; }
	}
}
