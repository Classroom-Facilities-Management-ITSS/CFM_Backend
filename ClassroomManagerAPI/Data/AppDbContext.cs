using ClassroomManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Facility> Facilities { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Facility>().HasData(new Facility
			{
				Id = 1,
				Name = "Microphone",
				Count = 1,
				Status = "Vacant",
				Version = "1.7",
				Description = "on-built teacher provided Microphone for teaching purposes"
			});

			modelBuilder.Entity<Facility>().HasData(new Facility
			{
				Id = 2,
				Name = "HDMI Cables",
				Count = 1,
				Status = "Malfunctioned",
				Version = "1.4",
				Description = "HDMI Cables connected to projector"
			});

			modelBuilder.Entity<Facility>().HasData(new Facility
			{
				Id = 3,
				Name = "Projector",
				Count = 1,
				Status = "Vacant",
				Version = "Sony VPL 4K",
				Description = "Projector connected to laptops via HDMI"
			});
		}
	}
}
