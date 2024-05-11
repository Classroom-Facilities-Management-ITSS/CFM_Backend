using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassroomManagerAPI.Configs.Infastructure
{
    public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Property(e => e.Status)
                .HasMaxLength(100)
                .HasConversion(v => v.ToString(), v => v.EnumParse<ClassroomStatusEnum>());
        }
    }
}
