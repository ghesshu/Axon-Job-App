using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Axon_Job_App.Migrations
{
    /// <inheritdoc />
    public partial class JobEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemporaryType",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "PaymentAmount",
                table: "Jobs",
                newName: "SalaryPerAnnum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalaryPerAnnum",
                table: "Jobs",
                newName: "PaymentAmount");

            migrationBuilder.AddColumn<string>(
                name: "TemporaryType",
                table: "Jobs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingHours",
                table: "Jobs",
                type: "TEXT",
                nullable: true);
        }
    }
}
