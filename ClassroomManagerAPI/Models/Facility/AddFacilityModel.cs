using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Facility
{
    public class AddFacilityModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Count { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
        public string Details { get; set; }
    }
}
