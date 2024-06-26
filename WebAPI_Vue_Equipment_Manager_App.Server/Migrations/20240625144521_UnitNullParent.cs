using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class UnitNullParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, null, "Administrator", null });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Address", "Building", "Name", "ParentId", "Room" },
                values: new object[] { 1, null, null, "Admin", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_ItemStatusCategory_Name",
                table: "ItemStatusCategory",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemStatusCategory_Name",
                table: "ItemStatusCategory");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { -1, null, "Administrator", null });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Address", "Building", "Name", "ParentId", "Room" },
                values: new object[] { -1, null, null, "Admin", null, null });
        }
    }
}
