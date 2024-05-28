using ClassroomManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassroomManagerAPI.Configs.Infastructure
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.HasOne(x => x.Classroom)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
