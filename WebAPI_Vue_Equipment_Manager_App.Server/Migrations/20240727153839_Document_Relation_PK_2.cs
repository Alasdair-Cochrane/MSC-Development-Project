using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI_Vue_Equipment_Manager_App.Server.Migrations
{
    /// <inheritdoc />
    public partial class Document_Relation_PK_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MaintenanceDocuments_MaintenanceId",
                table: "MaintenanceDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ModelDocuments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ItemDocuments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModelDocuments",
                table: "ModelDocuments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceDocuments",
                table: "MaintenanceDocuments",
                columns: new[] { "MaintenanceId", "DocumentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemDocuments",
                table: "ItemDocuments",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ModelDocuments",
                table: "ModelDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceDocuments",
                table: "MaintenanceDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemDocuments",
                table: "ItemDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ModelDocuments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ItemDocuments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceDocuments_MaintenanceId",
                table: "MaintenanceDocuments",
                column: "MaintenanceId");
        }
    }
}
