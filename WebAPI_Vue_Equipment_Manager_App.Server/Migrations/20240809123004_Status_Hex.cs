using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Status_Hex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "ItemStatusCategory",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 1,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 2,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 3,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 4,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 5,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 6,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 7,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 8,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 9,
                column: "ColorHex",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "ItemStatusCategory");
        }
    }
}
