﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassroomManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class lap_code : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilityAmount = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classrooms_Accounts_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facilities_Classrooms_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReporterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Accounts_ReporterID",
                        column: x => x.ReporterID,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Classrooms_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportedFacility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedFacility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportedFacility_Facilities_FacilityID",
                        column: x => x.FacilityID,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedFacility_Reports_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "Id", "ClassNumber", "CreatedAt", "FacilityAmount", "IsDeleted", "LastUsed", "ManagerId", "Note", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"), "402", new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9587), 2, false, null, null, null, new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9587) },
                    { new Guid("be72d844-8670-48e6-9d6b-859d407f2a5f"), "401", new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9581), 1, false, null, null, null, new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9581) }
                });

            migrationBuilder.InsertData(
                table: "Facilities",
                columns: new[] { "Id", "ClassID", "Count", "CreatedAt", "IsDeleted", "Name", "Note", "Status", "UpdatedAt", "Version" },
                values: new object[,]
                {
                    { new Guid("30d8a6ad-479c-42d7-b0f8-1e8f5270524c"), new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"), 1, new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9517), false, "HDMI Cables", "HDMI Cables connected to projector", "Malfunctioned", new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9517), "1.4" },
                    { new Guid("86843949-bb17-41e1-9d44-385d9d8c76c4"), new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"), 1, new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9520), false, "Projector", "Projector connected to laptops via HDMI", "Vacant", new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9520), "Sony VPL 4K" },
                    { new Guid("9b09d606-e706-4156-b065-2d3962d5ccae"), new Guid("be72d844-8670-48e6-9d6b-859d407f2a5f"), 1, new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9475), false, "Microphone", "on-built teacher provided Microphone for teaching purposes", "Vacant", new DateTime(2024, 5, 6, 4, 27, 48, 39, DateTimeKind.Local).AddTicks(9475), "1.7" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_ManagerId",
                table: "Classrooms",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_ClassID",
                table: "Facilities",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedFacility_FacilityID",
                table: "ReportedFacility",
                column: "FacilityID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedFacility_ReportID",
                table: "ReportedFacility",
                column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClassID",
                table: "Reports",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterID",
                table: "Reports",
                column: "ReporterID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportedFacility");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
