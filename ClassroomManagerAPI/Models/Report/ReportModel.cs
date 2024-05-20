using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Classroom;

namespace ClassroomManagerAPI.Models.Report
{
    public class ReportModel
    {
        public string? Note { get; set; }

        public AccountModel? Account { get; set; }
        public Guid? AccountId { get; set; }
        public ClassroomModel? Classroom { get; set; }
        public Guid? ClassroomId { get; set; }
        public IList<Guid>? ReportFacilities { get; set; }
    }
}
