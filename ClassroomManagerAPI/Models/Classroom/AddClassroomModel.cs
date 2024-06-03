using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Models.Classroom
{
	public class AddClassroomModel
	{
        public string? Address { get; set; }
        public DateTime LastUsed { get; set; }
        public int? MaxSize { get; set; } = 0;
        public string? Note { get; set; }
        public ClassroomStatusEnum Status { get; set; }
    }
}
