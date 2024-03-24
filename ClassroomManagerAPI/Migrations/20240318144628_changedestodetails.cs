using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class changedestodetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Facilities",
                newName: "Details");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Facilities",
                newName: "Description");
        }
    }
}
