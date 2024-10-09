using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterDonationAlleviationApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateAidTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisasterAidType_AidType_AidTypeId",
                table: "DisasterAidType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AidType",
                table: "AidType");

            migrationBuilder.RenameTable(
                name: "AidType",
                newName: "AidTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AidTypes",
                table: "AidTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DisasterAidType_AidTypes_AidTypeId",
                table: "DisasterAidType",
                column: "AidTypeId",
                principalTable: "AidTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisasterAidType_AidTypes_AidTypeId",
                table: "DisasterAidType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AidTypes",
                table: "AidTypes");

            migrationBuilder.RenameTable(
                name: "AidTypes",
                newName: "AidType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AidType",
                table: "AidType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DisasterAidType_AidType_AidTypeId",
                table: "DisasterAidType",
                column: "AidTypeId",
                principalTable: "AidType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
