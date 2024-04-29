using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagerAPI.Entities
{
    public class ReportedFacility :　BaseEntity
    {
        public Guid ReportID { get; set; } // foreign key reference report.id
        public Guid FacilityID { get; set; } // foreign key reference facility.id

        // Navigation Properties
        [ForeignKey(nameof(ReportID))]
        public Report Report { get; set; }
        [ForeignKey(nameof(FacilityID))]
        public Facility? Facility { get; set; }
    }
}
