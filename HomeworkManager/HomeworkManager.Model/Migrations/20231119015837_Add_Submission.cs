using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeworkManager.Model.Migrations
{
    /// <inheritdoc />
    public partial class Add_Submission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssignmentTypes",
                keyColumn: "AssignmentTypeId",
                keyValue: 3);

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.SubmissionId);
                    table.ForeignKey(
                        name: "FK_Submissions_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId");
                });

            migrationBuilder.CreateTable(
                name: "TextSubmissions",
                columns: table => new
                {
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextSubmissions", x => x.SubmissionId);
                    table.ForeignKey(
                        name: "FK_TextSubmissions_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "SubmissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AssignmentTypes",
                keyColumn: "AssignmentTypeId",
                keyValue: 1,
                column: "Name",
                value: "Text answer");

            migrationBuilder.UpdateData(
                table: "AssignmentTypes",
                keyColumn: "AssignmentTypeId",
                keyValue: 2,
                column: "Name",
                value: "File upload");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AssignmentId",
                table: "Submissions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentId",
                table: "Submissions",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextSubmissions");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.UpdateData(
                table: "AssignmentTypes",
                keyColumn: "AssignmentTypeId",
                keyValue: 1,
                column: "Name",
                value: "AnswerAssignment");

            migrationBuilder.UpdateData(
                table: "AssignmentTypes",
                keyColumn: "AssignmentTypeId",
                keyValue: 2,
                column: "Name",
                value: "FileUploadAssignment");

            migrationBuilder.InsertData(
                table: "AssignmentTypes",
                columns: new[] { "AssignmentTypeId", "Name" },
                values: new object[] { 3, "GitHubAssignment" });
        }
    }
}
