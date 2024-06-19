using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class statusCategoryNoSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemStatusCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Active" });
        }
    }
}
