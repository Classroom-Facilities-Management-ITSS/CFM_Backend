namespace ClassroomManagerAPI.Models.Dto
{
	public class UpdateFacilityRequestDto
	{
        public string Name { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
        public string? Note { get; set; }
    }
}
