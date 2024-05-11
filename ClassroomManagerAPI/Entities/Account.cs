using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Entities
{
    public class Account : BaseEntity
    {
        [Required(ErrorMessage = nameof(ErrorSystemEnum.Required))]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = nameof(ErrorSystemEnum.Required))]
        public string Password { get; set; }
        public string Role { get; set; } = Roles.USER;
        public bool Active { get; set; } = false;

        public UserInfo? UserInfo { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}
