using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Dto
{
	public class ForgotPasswordRequestDto
	{
		[EmailAddress]
		public string Email { get; set; }
	}
}
