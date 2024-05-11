namespace ClassroomManagerAPI.Models.Account
{
	public class UpdateAccountModel
	{
        public Guid Id { get; set; }
        public string Email { get; set; }
		public string Password { get; set; }
    }
}
