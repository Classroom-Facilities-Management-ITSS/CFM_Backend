using ClassroomManagerAPI.Models.Classroom;

namespace ClassroomManagerAPI.Models.Facility
{
    public class FacilityModel : AddFacilityModel
    {
        public Guid Id { get; set; }
        public ClassroomModel? Classroom { get; set; }
    }
}
