namespace ClassroomManagerAPI.Models.User
{
    public class UserModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? FullName { get; set; }
        public DateTime Dob { get; set; }
        public string? Department { get; set; }
        public string? Avatar { get; set; }
    }
}
