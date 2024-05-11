using ClassroomManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassroomManagerAPI.Configs.Infastructure
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.HasOne(x => x.Classroom)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
