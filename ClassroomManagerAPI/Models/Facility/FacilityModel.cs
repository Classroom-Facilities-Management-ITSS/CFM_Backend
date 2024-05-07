namespace ClassroomManagerAPI.Models.Facility
{
    public class FacilityModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
        public string? Note { get; set; }
    }
}
