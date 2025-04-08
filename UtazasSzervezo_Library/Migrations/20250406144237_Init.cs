using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtazasSzervezo_Library.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "max_person",
                table: "Accommodations",
                newName: "guests");

            migrationBuilder.AddColumn<string>(
                name: "departure_city",
                table: "Flights",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "departure_country",
                table: "Flights",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "destination_country",
                table: "Flights",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "detination_city",
                table: "Flights",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "planetype",
                table: "Flights",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "departure_city",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "departure_country",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "destination_country",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "detination_city",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "planetype",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "guests",
                table: "Accommodations",
                newName: "max_person");
        }
    }
}
