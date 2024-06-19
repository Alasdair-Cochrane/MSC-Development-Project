using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class DateOnly_to_DateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Models_EquipmentModelId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_EquipmentModelId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "EquipmentModelId",
                table: "Items");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Of_Reciept",
                table: "Items",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Of_Activation",
                table: "Items",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Of_Acceptance_Test",
                table: "Items",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "ItemStatusCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ModelId",
                table: "Items",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Models_ModelId",
                table: "Items",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Models_ModelId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ModelId",
                table: "Items");

            migrationBuilder.DeleteData(
                table: "ItemStatusCategory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date_Of_Reciept",
                table: "Items",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date_Of_Activation",
                table: "Items",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date_Of_Acceptance_Test",
                table: "Items",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquipmentModelId",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_EquipmentModelId",
                table: "Items",
                column: "EquipmentModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Models_EquipmentModelId",
                table: "Items",
                column: "EquipmentModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
