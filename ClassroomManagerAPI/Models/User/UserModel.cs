using ClassroomManagerAPI.Models.Account;

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
        public Guid? AccountId { get; set; }
        public AccountModel Account { get; set; }
    }
}
