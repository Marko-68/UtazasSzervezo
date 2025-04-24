using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtazasSzervezo_Library.Migrations
{
    /// <inheritdoc />
    public partial class AccommodationForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Accommodations_Accommodationid",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_Accommodationid",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Accommodationid",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_accommodation_id",
                table: "Bookings",
                column: "accommodation_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Accommodations_accommodation_id",
                table: "Bookings",
                column: "accommodation_id",
                principalTable: "Accommodations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Accommodations_accommodation_id",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_accommodation_id",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Accommodationid",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Accommodationid",
                table: "Bookings",
                column: "Accommodationid");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Accommodations_Accommodationid",
                table: "Bookings",
                column: "Accommodationid",
                principalTable: "Accommodations",
                principalColumn: "id");
        }
    }
}
