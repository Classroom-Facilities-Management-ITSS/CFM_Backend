namespace ClassroomManagerAPI.Models.Excels
{
    public class ValidateExcelModel
    {
        public int RowIndex { get; set; }

        public string? ColumnName { get; set; }

        public string? Message { get; set; }
    }
}
