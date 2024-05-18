using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: new Guid("42d228cd-fa56-49ba-b429-f753e34a01f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(2074), new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(2074) });

            migrationBuilder.UpdateData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: new Guid("be72d844-8670-48e6-9d6b-859d407f2a5f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(2068), new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(2068) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("30d8a6ad-479c-42d7-b0f8-1e8f5270524c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(2015), new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(2015) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("86843949-bb17-41e1-9d44-385d9d8c76c4"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(2018), new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(2018) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: new Guid("9b09d606-e706-4156-b065-2d3962d5ccae"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(1981), new DateTime(2024, 5, 17, 17, 16, 8, 28, DateTimeKind.Local).AddTicks(1981) });
        }
    }
}
