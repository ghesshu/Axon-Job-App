using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Axon_Job_App.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedClientWithImagePropertyV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoBase64",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoMimeType",
                table: "Clients",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoBase64",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LogoMimeType",
                table: "Clients");
        }
    }
}
