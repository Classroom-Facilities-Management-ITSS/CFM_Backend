namespace ClassroomManagerAPI.Models.Dto
{
	public class AccountDto
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
		public bool Active { get; set; }

	}
}
