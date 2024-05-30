using OfficeOpenXml.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Schedule
{
    public class ImportScheduleModel
    {
        [EpplusTableColumn(Header = "Môn Học")]
        public string? Subject { get; set; }
        [EpplusTableColumn(Header = "Lớp")]
        public string? Class { get; set; }
        [EpplusTableColumn(Header = "Bắt Đầu")]
        public string? StartTime { get; set; }
        [EpplusTableColumn(Header = "Kết Thúc")]
        public string? EndTime { get; set;}
        [EpplusTableColumn(Header = "Email")]
        [EmailAddress]
        public string? Email { get; set; }
        [EpplusTableColumn(Header = "Tuần Học")]
        public string? WeekStudy { get; set; }
        [EpplusTableColumn(Header = "Ngày Bắt Đầu Học")]
        public string? DateStart { get; set; }
        [EpplusTableColumn(Header = "Số Lượng SV")]
        public string? CountStudent { get; set; }
    }
}
