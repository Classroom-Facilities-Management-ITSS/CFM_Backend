namespace ClassroomManagerAPI.Models.Classroom
{
	public class ClassroomModel
	{
		public Guid? Id { get; set; }
		public string ClassNumber { get; set; }
		public string? LastUsed { get; set; }
		public int FacilityAmount { get; set; }
		public string? Note { get; set; }
	}
}
