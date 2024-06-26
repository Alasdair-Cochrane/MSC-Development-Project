using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class ItemComissioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_TypeId",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "Date_Of_Acceptance_Test",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Maintenances",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_TypeId",
                table: "Maintenances",
                newName: "IX_Maintenances_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Date_Of_Activation",
                table: "Items",
                newName: "Date_Of_Commissioning");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_CategoryId",
                table: "Maintenances",
                column: "CategoryId",
                principalTable: "MaintenanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_CategoryId",
                table: "Maintenances");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Maintenances",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Maintenances_CategoryId",
                table: "Maintenances",
                newName: "IX_Maintenances_TypeId");

            migrationBuilder.RenameColumn(
                name: "Date_Of_Commissioning",
                table: "Items",
                newName: "Date_Of_Activation");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_Of_Acceptance_Test",
                table: "Items",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_TypeId",
                table: "Maintenances",
                column: "TypeId",
                principalTable: "MaintenanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
