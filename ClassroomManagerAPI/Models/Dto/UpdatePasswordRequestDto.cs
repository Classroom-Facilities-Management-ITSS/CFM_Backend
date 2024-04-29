using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Dto
{
	public class UpdatePasswordRequestDto
	{
		[EmailAddress]
		[Required]
        public string Email { get; set; }
        [Required]
        public string OldPassword { get; set; }
		[Compare(nameof(NewPassword))]
		public string ConfirmPassword { get; set; }
		public string NewPassword { get; set; } = string.Empty;
	}
}
