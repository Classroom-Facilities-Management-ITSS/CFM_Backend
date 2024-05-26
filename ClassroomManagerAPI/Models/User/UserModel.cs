namespace ClassroomManagerAPI.Models.User
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? FullName
        {
            get { return $"{FirstName} {LastName}".Trim(); }
        }
        public DateTime? Dob { get; set; }
        public string Avatar { get; set; }
    }
}
