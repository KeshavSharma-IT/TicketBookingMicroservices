using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangesRefereshToken_to_RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefereshTokenExpiryTime",
                table: "Users",
                newName: "RefreshTokenExpiryTime");

            migrationBuilder.RenameColumn(
                name: "RefereshToken",
                table: "Users",
                newName: "RefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                newName: "RefereshTokenExpiryTime");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "Users",
                newName: "RefereshToken");
        }
    }
}
