using ClassroomManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassroomManagerAPI.Configs.Infastructure
{
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.HasOne(x => x.Account)
                    .WithOne(x => x.UserInfo)
                    .HasForeignKey<UserInfo>(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
