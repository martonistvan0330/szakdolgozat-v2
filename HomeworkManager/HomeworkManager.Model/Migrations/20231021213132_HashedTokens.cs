using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeworkManager.Model.Migrations
{
    /// <inheritdoc />
    public partial class HashedTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshTokens",
                newName: "TokenHash");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "AccessTokens",
                newName: "TokenHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "RefreshTokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "AccessTokens",
                newName: "Token");
        }
    }
}
