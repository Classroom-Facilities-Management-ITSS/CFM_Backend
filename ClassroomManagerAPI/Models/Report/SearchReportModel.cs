namespace ClassroomManagerAPI.Models.Report
{
	public class SearchReportModel : FilterModel
	{
		public string ClassroomAddress { get; set; }
		public string? AccountEmail { get; set; }
		public string? UserName { get; set; }
		public string? Note { get; set; }
	}
}
