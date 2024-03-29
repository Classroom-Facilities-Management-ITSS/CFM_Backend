using ClassroomManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Facility> Facilities { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Classroom> Classrooms { get; set; }
		public DbSet<Report> Reports { get; set; }
        public ReportedFacility ReportedFacilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seed data for Facility
			var facilities = new List<Facility>()
			{
				new Facility
				{
					Id = Guid.Parse("9b09d606-e706-4156-b065-2d3962d5ccae"),
					ClassID = Guid.Parse("be72d844-8670-48e6-9d6b-859d407f2a5f"),
					Name = "Microphone",
					Count = 1,
					Status = "Vacant",
					Version = "1.7",
					Note = "on-built teacher provided Microphone for teaching purposes"
				},
				new Facility
				{
					Id = Guid.Parse("30d8a6ad-479c-42d7-b0f8-1e8f5270524c"),
					Name = "HDMI Cables",
					ClassID = Guid.Parse("42d228cd-fa56-49ba-b429-f753e34a01f0"),
					Count = 1,
					Status = "Malfunctioned",
					Version = "1.4",
					Note = "HDMI Cables connected to projector"
				},
				new Facility
				{
					Id = Guid.Parse("86843949-bb17-41e1-9d44-385d9d8c76c4"),
					Name = "Projector",
					ClassID = Guid.Parse("42d228cd-fa56-49ba-b429-f753e34a01f0"),
					Count = 1,
					Status = "Vacant",
					Version = "Sony VPL 4K",
					Note = "Projector connected to laptops via HDMI"
				}
			};
			// Seed to the database
			modelBuilder.Entity<Facility>().HasData(facilities);

			// Seed data for classrooms
			var classrooms = new List<Classroom>()
			{
				new Classroom
				{
					Id = Guid.Parse("be72d844-8670-48e6-9d6b-859d407f2a5f"),
					ClassNumber = "401",
					LastUsed = null,
					FacilityAmount = 1,
					Note = null
				},
				new Classroom
				{
					Id = Guid.Parse("42d228cd-fa56-49ba-b429-f753e34a01f0"),
					ClassNumber = "402",
					LastUsed = null,
					FacilityAmount = 2,
					Note = null
				}
			};
			// Seed to the database
			modelBuilder.Entity<Classroom>().HasData(classrooms);
			// Seed data for Reports
			// Seed to the database

			// Seed data for ReportedFacility
			// Seed to the database

			// Seed data for accounts
			// Seed to the database
		}
	}
}
