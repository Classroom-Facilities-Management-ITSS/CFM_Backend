using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassroomManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedFacilityData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Facilities",
                columns: new[] { "Id", "Count", "Description", "Name", "Status", "Version" },
                values: new object[,]
                {
                    { 1, 1, "on-built teacher provided Microphone for teaching purposes", "Microphone", "Vacant", "1.7" },
                    { 2, 1, "HDMI Cables connected to projector", "HDMI Cables", "Malfunctioned", "1.4" },
                    { 3, 1, "Projector connected to laptops via HDMI", "Projector", "Vacant", "Sony VPL 4K" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
