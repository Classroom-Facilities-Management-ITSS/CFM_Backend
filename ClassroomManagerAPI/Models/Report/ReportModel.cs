using ClassroomManagerAPI.Models.Account;
using ClassroomManagerAPI.Models.Classroom;

namespace ClassroomManagerAPI.Models.Report
{
    public class ReportModel : AddReportModel
    {
        public Guid? Id { get; set; }
        public AccountModel? Account { get; set; }        
        public ClassroomModel? Classroom { get; set; }
        public IList<Guid>? ReportFacilities { get; set; }
    }
}
