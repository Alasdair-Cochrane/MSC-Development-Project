using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class TypeToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Roles_RoleId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Units_UnitId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_ItemDocuments_DocumentID",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Models_EquipmentTypes_TypeId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Models_TypeId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_DocumentID",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_RoleId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_UnitId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "DocumentID",
                table: "Maintenances");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "MaintenanceFrequencys",
                newName: "CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Models",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MaintenanceTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EquipmentTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Models_CategoryId",
                table: "Models",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceFrequencys_CategoryId",
                table: "MaintenanceFrequencys",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceFrequencys_MaintenanceTypes_CategoryId",
                table: "MaintenanceFrequencys",
                column: "CategoryId",
                principalTable: "MaintenanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Models_EquipmentTypes_CategoryId",
                table: "Models",
                column: "CategoryId",
                principalTable: "EquipmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceFrequencys_MaintenanceTypes_CategoryId",
                table: "MaintenanceFrequencys");

            migrationBuilder.DropForeignKey(
                name: "FK_Models_EquipmentTypes_CategoryId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Models_CategoryId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceFrequencys_CategoryId",
                table: "MaintenanceFrequencys");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Models");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "MaintenanceFrequencys",
                newName: "TypeId");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Models",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MaintenanceTypes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "DocumentID",
                table: "Maintenances",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EquipmentTypes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Models_TypeId",
                table: "Models",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_DocumentID",
                table: "Maintenances",
                column: "DocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_RoleId",
                table: "Assignments",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UnitId",
                table: "Assignments",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Roles_RoleId",
                table: "Assignments",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Units_UnitId",
                table: "Assignments",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_ItemDocuments_DocumentID",
                table: "Maintenances",
                column: "DocumentID",
                principalTable: "ItemDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_EquipmentTypes_TypeId",
                table: "Models",
                column: "TypeId",
                principalTable: "EquipmentTypes",
                principalColumn: "Id");
        }
    }
}
