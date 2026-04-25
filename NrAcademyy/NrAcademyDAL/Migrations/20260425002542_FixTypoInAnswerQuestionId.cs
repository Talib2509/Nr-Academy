using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NrAcademyDAL.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoInAnswerQuestionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QyestionId",
                table: "Answers",
                newName: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Answers",
                newName: "QyestionId");
        }
    }
}
