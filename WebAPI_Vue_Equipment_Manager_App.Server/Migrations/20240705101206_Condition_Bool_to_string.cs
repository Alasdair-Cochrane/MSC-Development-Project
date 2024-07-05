using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Condition_Bool_to_string : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "New_On_Reciept",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Condition_On_Reciept",
                table: "Items",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition_On_Reciept",
                table: "Items");

            migrationBuilder.AddColumn<bool>(
                name: "New_On_Reciept",
                table: "Items",
                type: "boolean",
                nullable: true);
        }
    }
}
