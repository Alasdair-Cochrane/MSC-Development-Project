using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Maintenance_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_CategoryId",
                table: "Maintenances");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Maintenances",
                newName: "MaintenanceCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_CategoryId",
                table: "Maintenances",
                newName: "IX_Maintenances_MaintenanceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_ItemId",
                table: "Maintenances",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Items_ItemId",
                table: "Maintenances",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_MaintenanceCategoryId",
                table: "Maintenances",
                column: "MaintenanceCategoryId",
                principalTable: "MaintenanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Items_ItemId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_MaintenanceCategoryId",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_ItemId",
                table: "Maintenances");

            migrationBuilder.RenameColumn(
                name: "MaintenanceCategoryId",
                table: "Maintenances",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_MaintenanceCategoryId",
                table: "Maintenances",
                newName: "IX_Maintenances_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_CategoryId",
                table: "Maintenances",
                column: "CategoryId",
                principalTable: "MaintenanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
