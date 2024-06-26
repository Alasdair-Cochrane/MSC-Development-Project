using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class LatestCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Assignments_RoleId",
                table: "Assignments",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UnitId",
                table: "Assignments",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetRoles_RoleId",
                table: "Assignments",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Units_UnitId",
                table: "Assignments",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetRoles_RoleId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Units_UnitId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_RoleId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_UnitId",
                table: "Assignments");
        }
    }
}
