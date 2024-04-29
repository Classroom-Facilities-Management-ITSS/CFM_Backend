using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagerAPI.Entities
{
    public class Report : BaseEntity
    {
        public Guid ReporterID { get; set; }
        public string Note { get; set; }
        public Guid? ClassID { get; set; }

        //foreign key reporterid int references account.id
        //foreign key classid references classroom.id

        // Navigation Properties
        [ForeignKey(nameof(ReporterID))]
        public Account Reporter { get; set; }
        [ForeignKey(nameof(ClassID))]
        public Classroom Classroom { get; set; }
        public ICollection<ReportedFacility>? ReportedFacilities { get; set; }

    }
}
