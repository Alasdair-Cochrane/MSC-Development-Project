using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class ItemStatusToEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Models_ModelId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ModelId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Current_Status",
                table: "Items");

            migrationBuilder.AlterColumn<decimal>(
                name: "Purchase_Price",
                table: "Items",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquipmentModelId",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemStatusCategoryId",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ItemStatusCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStatusCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_EquipmentModelId",
                table: "Items",
                column: "EquipmentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemStatusCategoryId",
                table: "Items",
                column: "ItemStatusCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemStatusCategory_ItemStatusCategoryId",
                table: "Items",
                column: "ItemStatusCategoryId",
                principalTable: "ItemStatusCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Models_EquipmentModelId",
                table: "Items",
                column: "EquipmentModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemStatusCategory_ItemStatusCategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Models_EquipmentModelId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemStatusCategory");

            migrationBuilder.DropIndex(
                name: "IX_Items_EquipmentModelId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemStatusCategoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "EquipmentModelId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemStatusCategoryId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "Purchase_Price",
                table: "Items",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Current_Status",
                table: "Items",
                type: "integer",
                nullable: true);

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
    }
}
