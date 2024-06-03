using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Models.Facility
{
    public class AddFacilityModel
    {
        public string? Name { get; set; }
        public int Count { get; set; }
        public FacilityStatusEnum? Status { get; set; }
        public string? Version { get; set; }
        public string? Note { get; set; }
        public Guid ClassroomId { get; set; }
    }
}
