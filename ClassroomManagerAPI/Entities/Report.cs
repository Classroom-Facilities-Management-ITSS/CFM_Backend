﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagerAPI.Entities
{
    public class Report : BaseEntity
    {
        public string? Note { get; set; }

        //foreign key reporterid int references account.id
        //foreign key classid references classroom.id

        // Navigation Properties
        
        public Account? Account { get; set; }
        public Guid? AccountId { get; set; }
        public Classroom? Classroom { get; set; }
        public Guid? ClassroomId { get; set; }
        public IList<Guid>? ReportFacilities { get; set; }
        //public IEnumerable<Facility> Facilities { get; set; }
    }
}
