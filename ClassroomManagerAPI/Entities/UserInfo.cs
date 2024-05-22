using ClassroomManagerAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagerAPI.Entities
{
	public class UserInfo : BaseEntity
	{
		[MaxLength(100, ErrorMessage = nameof(ErrorSystemEnum.MaxLength))]
        public string? FirstName { get; set; }
        [MaxLength(100, ErrorMessage = nameof(ErrorSystemEnum.MaxLength))]
        public string? LastName { get; set; }

		[NotMapped]
        public string? FullName {  
			get { return $"{FirstName} {LastName}".Trim();  }
		}
		public DateTime? Dob { get; set; }
		public string? Avatar { get; set; }
		public Account? Account { get; set; }
		public string? Department { get; set; }

		public Guid? AccountId { get; set; }
    }
}
