using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Document_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelDocuments",
                table: "ModelDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemDocuments",
                table: "ItemDocuments");

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ModelDocuments");

            migrationBuilder.DropColumn(
                name: "URL",
                table: "ModelDocuments");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ItemDocuments");

            migrationBuilder.DropColumn(
                name: "URL",
                table: "ItemDocuments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ModelDocuments",
                newName: "DocumentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ItemDocuments",
                newName: "DocumentId");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "ModelDocuments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "ItemDocuments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    URL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceDocuments",
                columns: table => new
                {
                    MaintenanceId = table.Column<int>(type: "integer", nullable: false),
                    DocumentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_MaintenanceDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceDocuments_Maintenances_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelDocuments_DocumentId",
                table: "ModelDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDocuments_DocumentId",
                table: "ItemDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceDocuments_DocumentId",
                table: "MaintenanceDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceDocuments_MaintenanceId",
                table: "MaintenanceDocuments",
                column: "MaintenanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDocuments_Documents_DocumentId",
                table: "ItemDocuments",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelDocuments_Documents_DocumentId",
                table: "ModelDocuments",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDocuments_Documents_DocumentId",
                table: "ItemDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelDocuments_Documents_DocumentId",
                table: "ModelDocuments");

            migrationBuilder.DropTable(
                name: "MaintenanceDocuments");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_ModelDocuments_DocumentId",
                table: "ModelDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ItemDocuments_DocumentId",
                table: "ItemDocuments");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "ModelDocuments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "ItemDocuments",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ModelDocuments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ModelDocuments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "ModelDocuments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ItemDocuments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ItemDocuments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "ItemDocuments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelDocuments",
                table: "ModelDocuments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemDocuments",
                table: "ItemDocuments",
                column: "Id");

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Analytical Balance" },
                    { 2, "Autoclave" },
                    { 3, "Bunsen Burner" },
                    { 4, "Desiccator" },
                    { 5, "Dry Heat Sterilizer" },
                    { 6, "Electron Microscope" },
                    { 7, "Fluorometer" },
                    { 8, "Fluorescence Microscope" },
                    { 9, "Fume Hood" },
                    { 10, "Gas Chromatograph" },
                    { 11, "Gel Electrophoresis System" },
                    { 12, "Homogenizer" },
                    { 13, "Heating Mantle" },
                    { 14, "Incubator" },
                    { 15, "Lab Freezer" },
                    { 16, "Laminar Flow Cabinet" },
                    { 17, "Light Microscope" },
                    { 18, "Liquid Chromatograph" },
                    { 19, "Microwave Oven" },
                    { 20, "Microcentrifuge" },
                    { 21, "Multi-Channel Pipette" },
                    { 22, "Pipette Filler" },
                    { 23, "Refrigerator" },
                    { 24, "Refractometer" },
                    { 25, "Spectrophotometer" },
                    { 26, "Shaker Incubator" },
                    { 27, "Sonicator" },
                    { 28, "Top-Loading Balance" },
                    { 29, "Vortex Mixer" },
                    { 30, "Water Bath" },
                    { 31, "Western Blot Apparatus" },
                    { 32, "Single-Channel Pipette" },
                    { 33, "Thermometer" },
                    { 34, "Floor Model Centrifuge" },
                    { 35, "Ultracentrifuge" }
                });
        }
    }
}
