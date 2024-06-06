using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Models.Classroom
{
	public class UpdateClassroomModel : AddClassroomModel
	{
        public Guid? Id { get; set; }
	}
}
