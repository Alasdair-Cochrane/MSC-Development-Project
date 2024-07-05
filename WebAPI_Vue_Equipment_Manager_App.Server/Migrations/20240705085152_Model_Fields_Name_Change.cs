using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Model_Fields_Name_Change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Models");

            migrationBuilder.RenameColumn(
                name: "Model_Number",
                table: "Models",
                newName: "ModelNumber");

            migrationBuilder.RenameColumn(
                name: "Model_Name",
                table: "Models",
                newName: "ModelName");

            migrationBuilder.RenameColumn(
                name: "Depth",
                table: "Models",
                newName: "Width");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Models",
                newName: "Depth");

            migrationBuilder.RenameColumn(
                name: "ModelNumber",
                table: "Models",
                newName: "Model_Number");

            migrationBuilder.RenameColumn(
                name: "ModelName",
                table: "Models",
                newName: "Model_Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Models",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Address", "Building", "Name", "ParentId", "Room" },
                values: new object[] { 1, null, null, "Admin", null, null });
        }
    }
}
