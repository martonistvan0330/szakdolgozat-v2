using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeworkManager.Model.Migrations
{
    /// <inheritdoc />
    public partial class Add_PasswordRecoveryToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordRecoveryTokens",
                columns: table => new
                {
                    PasswordRecoveryTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecoveryTokens", x => x.PasswordRecoveryTokenId);
                    table.ForeignKey(
                        name: "FK_PasswordRecoveryTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordRecoveryTokens_UserId",
                table: "PasswordRecoveryTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordRecoveryTokens");
        }
    }
}
