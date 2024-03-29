using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagerAPI.Models
{
	public class Facility
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public int Count { get; set; }
		public string Status { get; set; }
        public string Version { get; set; }
        public string? Note { get; set; }
        public Guid? ClassID { get; set; }

        // Navigation property for related Classroom
        [ForeignKey(nameof(ClassID))]
        public Classroom? Classroom { get; set; }
        public ICollection<ReportedFacility>? ReportedFacilities { get; set; }

    }
}
