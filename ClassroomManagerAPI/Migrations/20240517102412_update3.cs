using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7670), new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7670) });

            migrationBuilder.UpdateData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: new Guid("be72d844-8670-48e6-9d6b-859d407f2a5f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7666), new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7666) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("30d8a6ad-479c-42d7-b0f8-1e8f5270524c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7597), new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7597) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("86843949-bb17-41e1-9d44-385d9d8c76c4"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7601), new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7601) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("9b09d606-e706-4156-b065-2d3962d5ccae"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7560), new DateTime(2024, 5, 17, 17, 24, 10, 960, DateTimeKind.Local).AddTicks(7560) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(5009), new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(5009) });

            migrationBuilder.UpdateData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: new Guid("be72d844-8670-48e6-9d6b-859d407f2a5f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(5005), new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(5005) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("30d8a6ad-479c-42d7-b0f8-1e8f5270524c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(4940), new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(4940) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("86843949-bb17-41e1-9d44-385d9d8c76c4"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(4944), new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(4944) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("9b09d606-e706-4156-b065-2d3962d5ccae"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(4902), new DateTime(2024, 5, 17, 17, 19, 54, 794, DateTimeKind.Local).AddTicks(4902) });
        }
    }
}
