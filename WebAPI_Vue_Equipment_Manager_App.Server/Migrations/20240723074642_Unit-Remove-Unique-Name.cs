using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class UnitRemoveUniqueName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_Name",
                table: "Units");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Private User");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Public User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "PrivateUser");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "PublicUser");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 4, null, "TemporaryUser", "TEMP" });

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { -1, "Centrifuge" });

            migrationBuilder.CreateIndex(
                name: "IX_Units_Name",
                table: "Units",
                column: "Name",
                unique: true);
        }
    }
}
