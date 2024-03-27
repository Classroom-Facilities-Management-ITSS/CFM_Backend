using System.ComponentModel.DataAnnotations;

namespace CFM.Web.Models
{
	public class FacilityDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Count { get; set; }
		public string Status { get; set; }
		public string Version { get; set; }
		public string Details { get; set; }
	}
}
