using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Axon_Job_App.Migrations
{
    /// <inheritdoc />
    public partial class clientData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfAttendingCandidates",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfAttendingCandidates",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
