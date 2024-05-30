using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Entities
{
    public class BaseEntity
    {
        [Key]
        public virtual Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        protected BaseEntity() { 
            Id = Guid.Empty;
            CreatedAt = UpdatedAt = DateTime.Now;
        }
    }
}
