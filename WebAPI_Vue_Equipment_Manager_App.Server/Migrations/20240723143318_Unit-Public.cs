using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class UnitPublic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Units",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Units");
        }
    }
}
