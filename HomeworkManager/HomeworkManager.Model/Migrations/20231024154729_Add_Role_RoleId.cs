using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeworkManager.Model.Migrations
{
    /// <inheritdoc />
    public partial class Add_Role_RoleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "AspNetRoles");
        }
    }
}
