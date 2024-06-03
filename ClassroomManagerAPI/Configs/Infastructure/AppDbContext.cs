using ClassroomManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Configs.Infastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSet
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new UserInfoConfiguration());
            modelBuilder.ApplyConfiguration(new ClassroomConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityConfigurarion());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            base.OnModelCreating(modelBuilder);
            FacilitySeed(modelBuilder);
            ClassroomSeed(modelBuilder);
            // Seed data for Reports
            // Seed to the database

            // Seed data for ReportedFacility
            // Seed to the database

            // Seed data for accounts
            // Seed to the database
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ArgumentNullException.ThrowIfNull(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile(Settings.SettingFileName)
                    .Build();
                optionsBuilder.UseSqlServer(
                    configuration.GetConnectionString(Settings.DefaultConnection),
                    options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));
            }
        }
        #region DataSeed
        private static void FacilitySeed(ModelBuilder modelBuilder)
        {
            // Seed data for Facility
            var facilities = new List<Facility>()
            {
                new Facility
                {
                    Id = Guid.Parse("9b09d606-e706-4156-b065-2d3962d5ccae"),
                    ClassroomId = Guid.Parse("be72d844-8670-48e6-9d6b-859d407f2a5f"),
                    Name = "Microphone",
                    Count = 1,
                    Status = Enums.FacilityStatusEnum.NEW,
                    Version = "1.7",
                    Note = "on-built teacher provided Microphone for teaching purposes"
                },
                new Facility
                {
                    Id = Guid.Parse("30d8a6ad-479c-42d7-b0f8-1e8f5270524c"),
                    Name = "HDMI Cables",
                    ClassroomId = Guid.Parse("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                    Count = 1,
                    Status = Enums.FacilityStatusEnum.OLD,
                    Version = "1.4",
                    Note = "HDMI Cables connected to projector"
                },
                new Facility
                {
                    Id = Guid.Parse("86843949-bb17-41e1-9d44-385d9d8c76c4"),
                    Name = "Projector",
                    ClassroomId = Guid.Parse("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                    Count = 1,
                    Status = Enums.FacilityStatusEnum.FIXING,
                    Version = "Sony VPL 4K",
                    Note = "Projector connected to laptops via HDMI"
                }
            };
            // Seed to the database
            modelBuilder.Entity<Facility>().HasData(facilities);
        }

        private static void ClassroomSeed(ModelBuilder modelBuilder)
        {
            // Seed data for classrooms
            var classrooms = new List<Classroom>()
            {
                new Classroom
                {
                    Id = Guid.Parse("be72d844-8670-48e6-9d6b-859d407f2a5f"),
                    Address = "D9-401",
                    LastUsed = DateTime.Now,
                    FacilityAmount = 1,
                    Note = null
                },
                new Classroom
                {
                    Id = Guid.Parse("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                    Address = "D9-402",
                    LastUsed = DateTime.Now,
                    FacilityAmount = 2,
                    Note = null
                },
                new Classroom
                {
                    Id = Guid.Parse("0540dec7-c15c-4d3d-9b24-a1cfca346209"),
                    Address = Configs.Settings.StorageClass,
                    LastUsed = DateTime.Now,
                    FacilityAmount = 0,
                    Note = null
                }
            };
            // Seed to the database
            modelBuilder.Entity<Classroom>().HasData(classrooms);
        }
    }
    #endregion
}
