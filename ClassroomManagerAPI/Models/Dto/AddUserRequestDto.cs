using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models.Dto
{
	public class AddUserRequestDto
	{
        [Required]
        [EmailAddress]
        public string Email { get; set; }
		[Required]
		[MinLength(8)]
		public string Password { get; set; }
    }
}
