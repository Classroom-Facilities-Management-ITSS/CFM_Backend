namespace ClassroomManagerAPI.Models.Facility
{
	public class SearchFacilityModel : FilterModel
	{
		public string Name { get; set; }
		public int? Count { get; set; }
		public string? Version { get; set; }
		public string? ClassroomAddress { get; set; }
	}
}
