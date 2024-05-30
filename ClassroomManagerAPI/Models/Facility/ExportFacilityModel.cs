using ClassroomManagerAPI.Enums;
using OfficeOpenXml.Attributes;

namespace ClassroomManagerAPI.Models.Facility
{
    public class ExportFacilityModel
    {
        [EpplusTableColumn(Header = "Mã")]
        public Guid? Id { get; set; }
        [EpplusTableColumn(Header = "Tên thiết bị")]
        public string? Name { get; set; }
        [EpplusTableColumn(Header = "Số lượng")]
        public int? Count { get; set; }
        [EpplusTableColumn(Header = "Tình trạng")]
        public FacilityStatusEnum? Status { get; set; }
        [EpplusTableColumn(Header = "Phiên bản")]
        public string? Version { get; set; }
        [EpplusTableColumn(Header = "Ghi chú")]
        public string? Note { get; set; }
        [EpplusTableColumn(Header = "Lơp")]
        public string? ClassroomAdrress { get; set; }
        [EpplusTableColumn(Header = "Lần sử dụng cuối")]
        public DateTime? LastUsed { get; set; }
    }
}
