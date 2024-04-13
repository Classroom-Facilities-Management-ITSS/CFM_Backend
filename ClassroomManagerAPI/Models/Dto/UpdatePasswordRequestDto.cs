namespace ClassroomManagerAPI.Models.Dto
{
	public class UpdatePasswordRequestDto
	{
        public string Email { get; set; }
		public string OldPassword { get; set; }
		public string ConfirmPassword { get; set; }
		public string NewPassword { get; set; } = string.Empty;
	}
}
