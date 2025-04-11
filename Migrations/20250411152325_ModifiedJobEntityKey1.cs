using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Axon_Job_App.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedJobEntityKey1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAssignments_Jobs_JobId",
                table: "JobAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobAssignments",
                table: "JobAssignments");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "RolePermissions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<long>(
                name: "JobId",
                table: "JobAssignments",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "JobId1",
                table: "JobAssignments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobAssignments",
                table: "JobAssignments",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_JobId1",
                table: "JobAssignments",
                column: "JobId1");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAssignments_Jobs_JobId1",
                table: "JobAssignments",
                column: "JobId1",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAssignments_Jobs_JobId1",
                table: "JobAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobAssignments",
                table: "JobAssignments");

            migrationBuilder.DropIndex(
                name: "IX_JobAssignments_JobId1",
                table: "JobAssignments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "JobId1",
                table: "JobAssignments");

            migrationBuilder.AlterColumn<long>(
                name: "JobId",
                table: "JobAssignments",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobAssignments",
                table: "JobAssignments",
                columns: new[] { "JobId", "CandidateId" });

            migrationBuilder.AddForeignKey(
                name: "FK_JobAssignments_Jobs_JobId",
                table: "JobAssignments",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
