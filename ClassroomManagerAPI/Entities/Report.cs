namespace ClassroomManagerAPI.Entities
{
    public class Report : BaseEntity
    {
        public string? Note { get; set; }
        
        public Account? Account { get; set; }
        public Guid? AccountId { get; set; }
        public Classroom? Classroom { get; set; }
        public Guid? ClassroomId { get; set; }
        public IList<Guid>? ReportFacilities { get; set; }
    }
}
