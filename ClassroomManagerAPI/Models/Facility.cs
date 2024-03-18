using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models
{
	public class Facility
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public int Count { get; set; }
		public string Status { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
    }
}
