namespace ClassroomManagerAPI.Models.Report
{
	public class SearchReportModel : FilterModel
	{
		public string ClassroomAddress { get; set; }
		public string? Email { get; set; }
		public string? FullName { get; set; }
		public string? Note { get; set; }
	}
}
