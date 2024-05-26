using ClassroomManagerAPI.Models.User;

namespace ClassroomManagerAPI.Models.Account
{
	public class AccountModel
	{
		public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public UserModel User { get; set; }
    }
}
