using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Entities
{
    public class Classroom : BaseEntity
    {
        [Required]
        public string ClassNumber { get; set; }
        public string? LastUsed { get; set; }
        public int FacilityAmount { get; set; }
        public string? Note { get; set; }
        public Guid? ManagerId { get; set; }
        //managerId int
        // foreignkey managerid references account.id

        // Navigation property for related manager account
        public Account? Manager { get; set; }

        // Navigation property for related facilities
        public ICollection<Facility>? Facilities { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}
