using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterDonationAlleviationApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDisasterModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Severity",
                table: "Disasters",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Disasters");
        }
    }
}
