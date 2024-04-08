using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Models
{
	public class Account
	{
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; } 
    }
}
