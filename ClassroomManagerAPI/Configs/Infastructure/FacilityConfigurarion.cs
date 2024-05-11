using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassroomManagerAPI.Configs.Infastructure
{
    public class FacilityConfigurarion : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.HasOne(x => x.Classroom)
                .WithMany(x => x.Facilities)
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Status)
                .HasConversion(v => v.ToString(), v => v.EnumParse<FacilityStatusEnum>());
        }
    }
}
