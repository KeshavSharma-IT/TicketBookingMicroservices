using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventServices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeUniqueIndexesToVenueAndScreen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Screen_VenueId",
                table: "Screen");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Venues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Venues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Venues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Screen",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Venues_Name_City_Address",
                table: "Venues",
                columns: new[] { "Name", "City", "Address" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Screen_VenueId_Name",
                table: "Screen",
                columns: new[] { "VenueId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Venues_Name_City_Address",
                table: "Venues");

            migrationBuilder.DropIndex(
                name: "IX_Screen_VenueId_Name",
                table: "Screen");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Screen",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Screen_VenueId",
                table: "Screen",
                column: "VenueId");
        }
    }
}
