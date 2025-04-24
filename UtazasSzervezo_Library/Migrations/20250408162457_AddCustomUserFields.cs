using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtazasSzervezo_Library.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "phone_number",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "postalcode",
                table: "Users",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "postalcode",
                table: "Users");
        }
    }
}
