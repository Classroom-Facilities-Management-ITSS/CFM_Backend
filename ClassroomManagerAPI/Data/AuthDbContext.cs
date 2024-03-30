using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Data
{
	public class AuthDbContext : IdentityDbContext
	{
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var staffRoleId = "ce29186e-ee52-4b55-b803-8ca6fa82e3ed"; // nhan vien kiem tra phong hoc
            var managerRoleId = "689f725b-58be-40e6-85de-e3b52e3ed1d8"; // quan ly phong hoc 

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = staffRoleId,
                    ConcurrencyStamp = staffRoleId,
                    Name = "Staff",
                    NormalizedName = "Staff".ToUpper()
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    ConcurrencyStamp = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "Manager".ToUpper()
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
