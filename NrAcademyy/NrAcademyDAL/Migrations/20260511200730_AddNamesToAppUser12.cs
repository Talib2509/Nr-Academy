using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NrAcademyDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNamesToAppUser12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_AspNetUsers_UserId",
                table: "StudentCourses");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_AspNetUsers_UserId",
                table: "StudentCourses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_TeacherUserId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_AspNetUsers_UserId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TeacherUserId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeacherUserId",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_AspNetUsers_UserId",
                table: "StudentCourses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
