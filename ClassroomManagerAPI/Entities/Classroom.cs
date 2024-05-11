using ClassroomManagerAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Entities
{
    public class Classroom : BaseEntity
    {
        [Required(ErrorMessage = nameof(ErrorSystemEnum.Required))]
        public string Address { get; set; }
        public string? LastUsed { get; set; }
        public int FacilityAmount { get; set; }
        public string? Note { get; set; }
        public ClassroomStatusEnum Status { get; set; }
        //managerId int
        // foreignkey managerid references account.id

        // Navigation property for related manager account

        // Navigation property for related facilities
        public ICollection<Facility>? Facilities { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}
