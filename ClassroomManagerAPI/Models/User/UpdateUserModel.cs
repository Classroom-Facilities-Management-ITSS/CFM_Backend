namespace ClassroomManagerAPI.Models.User
{
    public class UpdateUserModel
    {
        public Guid? AccountId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Department { get; set; }
        public string? Avatar { get; set; }
    }
}
