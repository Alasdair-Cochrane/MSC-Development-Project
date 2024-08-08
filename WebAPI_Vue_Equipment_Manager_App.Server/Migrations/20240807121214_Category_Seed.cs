using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Category_Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemStatusCategory",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[,]
                {
                    { 1, "Active", 1 },
                    { 2, "In Storage", 2 },
                    { 3, "Broken", 3 },
                    { 4, "Decommissioned", 4 },
                    { 5, "On Loan", 5 },
                    { 6, "Requires Installation", 6 },
                    { 7, "Requires Service", 7 },
                    { 8, "Requires Calibration", 8 },
                    { 9, "Requires Validation", 9 }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Repair" },
                    { 2, "Calibration" },
                    { 3, "Service" },
                    { 4, "Validation" },
                    { 5, "Decommissioning" },
                    { 6, "Decontamination" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MaintenanceTypes",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
