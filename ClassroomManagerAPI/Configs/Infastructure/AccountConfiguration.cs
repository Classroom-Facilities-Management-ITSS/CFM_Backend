using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassroomManagerAPI.Configs.Infastructure
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

			builder.Property(e => e.Role)
				.HasMaxLength(100)
				.HasConversion(v => v.ToString(), v => v.EnumParse<RoleEnum>());
		}
    }
}
