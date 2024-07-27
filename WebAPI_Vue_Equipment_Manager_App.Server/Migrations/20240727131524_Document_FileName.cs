using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Document_FileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ModelDocuments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ItemDocuments",
                type: "text",
                nullable: false,
                defaultValue: "");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ModelDocuments");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ItemDocuments");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Single-Channel Pipette");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Multi-Channel Pipette");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Pipette Filler");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Microcentrifuge");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Floor Model Centrifuge");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Ultracentrifuge");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Lab Freezer");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Refrigerator");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Light Microscope");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Electron Microscope");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Fluorescence Microscope");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Spectrophotometer");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "Fluorometer");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "Hot Plate");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Name",
                value: "Bunsen Burner");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "Heating Mantle");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "Autoclave");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "Dry Heat Sterilizer");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "Incubator");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "Shaker Incubator");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "Analytical Balance");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 22,
                column: "Name",
                value: "Top-Loading Balance");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "Vortex Mixer");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "Water Bath");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "Thermometer");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Fume Hood");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "Laminar Flow Cabinet");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 28,
                column: "Name",
                value: "Homogenizer");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 29,
                column: "Name",
                value: "Sonicator");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 30,
                column: "Name",
                value: "Gas Chromatograph");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 31,
                column: "Name",
                value: "Liquid Chromatograph");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 32,
                column: "Name",
                value: "Gel Electrophoresis System");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 33,
                column: "Name",
                value: "Western Blot Apparatus");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 34,
                column: "Name",
                value: "Refractometer");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 35,
                column: "Name",
                value: "Desiccator");

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 36, "Microwave Oven" });
        }
    }
}
