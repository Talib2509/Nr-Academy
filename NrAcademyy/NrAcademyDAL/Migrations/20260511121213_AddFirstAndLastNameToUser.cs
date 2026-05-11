using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NrAcademyDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstAndLastNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ComletedAt",
                table: "TestResults",
                newName: "StartedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActiveDate",
                table: "Tests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestType",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "TestResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsWinner",
                table: "TestResults",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_UserId",
                table: "TestResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Question_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_AspNetUsers_UserId",
                table: "TestResults",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Question_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_AspNetUsers_UserId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_UserId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "ActiveDate",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TestType",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "IsWinner",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "TestResults",
                newName: "ComletedAt");
        }
    }
}
