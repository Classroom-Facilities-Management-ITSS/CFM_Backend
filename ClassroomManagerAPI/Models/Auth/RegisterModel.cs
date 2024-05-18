using ClassroomManagerAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Auth
{
	public class RegisterModel
	{
		[EmailAddress]
		public string Email { get; set; }
		[MinLength(8, ErrorMessage = nameof(ErrorSystemEnum.MinLength))]
		public string Password { get; set; }

	}
}
