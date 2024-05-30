namespace ClassroomManagerAPI.Models.Report
{
	public class AddReportModel
	{
		public string? Note { get; set; }
		public Guid? AccountId { get; set; }
		public Guid? ClassroomId { get; set; }
		public IList<Guid>? ReportFacilities { get; set; }
	}
}
