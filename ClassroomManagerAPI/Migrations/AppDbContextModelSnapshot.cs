﻿// <auto-generated />
using System;
using ClassroomManagerAPI.Configs.Infastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClassroomManagerAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Classroom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("FacilityAmount")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUsed")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxSize")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Classrooms");

                    b.HasData(
                        new
                        {
                            Id = new Guid("be72d844-8670-48e6-9d6b-859d407f2a5f"),
                            Address = "D9-401",
                            CreatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7221),
                            FacilityAmount = 1,
                            IsDeleted = false,
                            LastUsed = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7223),
                            MaxSize = 0,
                            Status = "OPEN",
                            UpdatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7221)
                        },
                        new
                        {
                            Id = new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                            Address = "D9-402",
                            CreatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7227),
                            FacilityAmount = 2,
                            IsDeleted = false,
                            LastUsed = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7228),
                            MaxSize = 0,
                            Status = "OPEN",
                            UpdatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7227)
                        });
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Facility", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClassroomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.ToTable("Facilities");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9b09d606-e706-4156-b065-2d3962d5ccae"),
                            ClassroomId = new Guid("be72d844-8670-48e6-9d6b-859d407f2a5f"),
                            Count = 1,
                            CreatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7123),
                            IsDeleted = false,
                            Name = "Microphone",
                            Note = "on-built teacher provided Microphone for teaching purposes",
                            Status = "NEW",
                            UpdatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7123),
                            Version = "1.7"
                        },
                        new
                        {
                            Id = new Guid("30d8a6ad-479c-42d7-b0f8-1e8f5270524c"),
                            ClassroomId = new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                            Count = 1,
                            CreatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7154),
                            IsDeleted = false,
                            Name = "HDMI Cables",
                            Note = "HDMI Cables connected to projector",
                            Status = "OLD",
                            UpdatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7154),
                            Version = "1.4"
                        },
                        new
                        {
                            Id = new Guid("86843949-bb17-41e1-9d44-385d9d8c76c4"),
                            ClassroomId = new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                            Count = 1,
                            CreatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7159),
                            IsDeleted = false,
                            Name = "Projector",
                            Note = "Projector connected to laptops via HDMI",
                            Status = "FIXING",
                            UpdatedAt = new DateTime(2024, 6, 1, 15, 19, 13, 767, DateTimeKind.Local).AddTicks(7159),
                            Version = "Sony VPL 4K"
                        });
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClassroomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReportFacilities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ClassroomId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Schedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClassroomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CountStudent")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ClassroomId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Facility", b =>
                {
                    b.HasOne("ClassroomManagerAPI.Entities.Classroom", "Classroom")
                        .WithMany("Facilities")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Classroom");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Report", b =>
                {
                    b.HasOne("ClassroomManagerAPI.Entities.Account", "Account")
                        .WithMany("Reports")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ClassroomManagerAPI.Entities.Classroom", "Classroom")
                        .WithMany("Reports")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Account");

                    b.Navigation("Classroom");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Schedule", b =>
                {
                    b.HasOne("ClassroomManagerAPI.Entities.Account", "Account")
                        .WithMany("Schedules")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ClassroomManagerAPI.Entities.Classroom", "Classroom")
                        .WithMany("Schedules")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Account");

                    b.Navigation("Classroom");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.UserInfo", b =>
                {
                    b.HasOne("ClassroomManagerAPI.Entities.Account", "Account")
                        .WithOne("UserInfo")
                        .HasForeignKey("ClassroomManagerAPI.Entities.UserInfo", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Account", b =>
                {
                    b.Navigation("Reports");

                    b.Navigation("Schedules");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("ClassroomManagerAPI.Entities.Classroom", b =>
                {
                    b.Navigation("Facilities");

                    b.Navigation("Reports");

                    b.Navigation("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
