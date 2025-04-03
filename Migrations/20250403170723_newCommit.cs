using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Axon_Job_App.Migrations
{
    /// <inheritdoc />
    public partial class newCommit : Migration
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

            migrationBuilder.RenameColumn(
                name: "CompanyImage",
                table: "Clients",
                newName: "Website");

            migrationBuilder.AddColumn<string>(
                name: "CeoFirstName",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CeoLastName",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyAddress",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmail",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogo",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPhone",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationCoordinates",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CeoFirstName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CeoLastName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CompanyAddress",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CompanyEmail",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CompanyLogo",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CompanyPhone",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LocationCoordinates",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "SalaryPerAnnum",
                table: "Jobs",
                newName: "PaymentAmount");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Clients",
                newName: "CompanyImage");

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
