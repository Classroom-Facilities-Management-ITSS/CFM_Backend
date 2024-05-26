namespace ClassroomManagerAPI.Models.Account
{
    public class SearchAccountModel : FilterModel
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
    }
}
