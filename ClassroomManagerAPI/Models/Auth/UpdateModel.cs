using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Auth
{
	public class UpdateModel
	{
		[EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string OldPassword { get; set; }
		[Compare(nameof(NewPassword))]
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
