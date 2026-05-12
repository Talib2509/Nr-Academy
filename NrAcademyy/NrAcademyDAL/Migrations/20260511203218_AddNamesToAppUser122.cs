using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NrAcademyDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNamesToAppUser122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_TeacherUserId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TeacherUserId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeacherUserId",
                table: "Courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherUserId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherUserId",
                table: "Courses",
                column: "TeacherUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_TeacherUserId",
                table: "Courses",
                column: "TeacherUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
