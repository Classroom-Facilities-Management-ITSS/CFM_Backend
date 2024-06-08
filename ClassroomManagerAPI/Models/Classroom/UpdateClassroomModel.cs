using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Models.Classroom
{
	public class UpdateClassroomModel 
	{
        public Guid? Id { get; set; }
		public string? Address { get; set; }
		public DateTime LastUsed { get; set; }
		public string? Note { get; set; }
		public ClassroomStatusEnum? Status { get; set; }
	}
}
