using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Facility
{
    public class FacilityModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
        public string Details { get; set; }
    }
}
